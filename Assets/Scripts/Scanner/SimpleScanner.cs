using System;
using System.Threading;
using BarcodeScanner.Parser;
using BarcodeScanner.Webcam;
using UnityEngine;
using Wizcorp.Utils.Logger;
using ZXing;

namespace BarcodeScanner.Scanner
{
	public class Scanner : IScanner
	{
		//
		public event EventHandler OnReady;
		public event EventHandler StatusChanged;

		public IWebcam Camera { get; private set; }
		public IParser Parser { get; private set; }

		private ScannerStatus status;
		public ScannerStatus Status {
			get
			{
				return status;
			}
			private set
			{
				status = value;
				if (StatusChanged != null)
				{
					StatusChanged.Invoke(this, EventArgs.Empty);
				}
			}
		}
		

		//
		private BarcodeReader scanner = new BarcodeReader();
		private Color32[] pixels = new Color32[0];

		//
		
		private Action<string, string> Callback;
		private ParserResult Result;

		public Scanner(IParser parser = null, IWebcam webcam = null)
		{
			Status = ScannerStatus.Initialize;
			Parser = (parser == null) ? new ZXingParser() : parser;
			Camera = (webcam == null) ? new UnityWebcam() : webcam;
		}

		public void Scan(Action<string, string> callback)
		{
			if (Callback != null)
			{
				Log.Warning("Already Scan");
				return;
			}
			Callback = callback;

			Log.Info("SimpleScanner -> Start Scan");
			Status = ScannerStatus.Running;
			CodeScannerThread = new Thread(DecodeQR);
			CodeScannerThread.Start();
		}

		public void Stop()
		{
			if (Callback == null)
			{
				Log.Warning("No Scan running");
				return;
			}

			// Stop thread / Clean callback
			Log.Info("SimpleScanner -> Stop Scan");
			if (CodeScannerThread != null)
			{
				CodeScannerThread.Abort();
			}
			Callback = null;
			Status = ScannerStatus.Paused;
		}

		#region Background Thread

		private Thread CodeScannerThread;

		public void DecodeQR()
		{
			while (Result == null)
			{
				// Wait
				if (Status != ScannerStatus.Running || pixels.Length == 00 || Camera.Width == 0)
				{
					Thread.Sleep(100);
					continue;
				}

				// Process
				Log.Info("SimpleScanner -> Scan ... " + Camera.Width + " / " + Camera.Height);
				try
				{
					Result = Parser.Decode(pixels, Camera.Width, Camera.Height);
					pixels = null;

					// Sleep a little bit and set the signal to get the next frame
					Thread.Sleep(100);
				}
				catch (Exception e)
				{
					Log.Error(e);
				}
			}
		}

		#endregion

		public void Update()
		{
			if (!Camera.IsReady())
			{
				Log.Info("Camera Not Ready Yet ...");
				if (status != ScannerStatus.Initialize)
				{
					Status = ScannerStatus.Initialize;
				}
				return;
			}

			if (Status == ScannerStatus.Initialize)
			{
				Status = ScannerStatus.Paused;

				Camera.SetSize();
				if (OnReady != null)
				{
					OnReady.Invoke(this, EventArgs.Empty);
				}
			}

			if (Status == ScannerStatus.Running)
			{
				pixels = Camera.GetPixels();

				if (Result != null)
				{
					// Call callback
					Callback(Result.Type, Result.Value);
					Result = null;
					pixels = new Color32[0];
				}
			}
		}
	}
}
