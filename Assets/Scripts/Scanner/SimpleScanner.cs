using System;
using System.Threading;
using BarcodeScanner.Parser;
using BarcodeScanner.Webcam;
using UnityEngine;

namespace BarcodeScanner.Scanner
{
	public class SimpleScanner : IScanner
	{
		//
		public event EventHandler StatusChanged;
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
		public IWebcam Camera { get; private set; }
		public IParser Parser { get; private set; }

		//
		private Color32[] Pixels = null;
		private int cameraWidth, cameraHeight = 0;
		//
		private Thread CodeScannerThread;
		private Action<string, string> Callback;
		private ParserResult Result;

		public SimpleScanner(IParser parser = null, IWebcam webcam = null)
		{
			Status = ScannerStatus.Initialize;
			Parser = (parser == null) ? new ZXingParser() : parser;
			Camera = (webcam == null) ? new UnityWebcam() : webcam;
		}

		public void Scan(Action<string, string> callback)
		{
			if (Callback != null)
			{
				Debug.LogWarning("Already Scan");
				return;
			}
			Callback = callback;

			Debug.Log("SimpleScanner -> Start Scan");
			Status = ScannerStatus.Running;
			CodeScannerThread = new Thread(DecodeQR);
			CodeScannerThread.Start();
		}

		public void Stop()
		{
			if (Callback == null)
			{
				Debug.LogWarning("No Scan running");
				return;
			}

			// Stop thread / Clean callback
			Debug.Log("SimpleScanner -> Stop Scan");
			if (CodeScannerThread != null)
			{
				CodeScannerThread.Abort();
			}
			Callback = null;
			Status = ScannerStatus.Paused;
		}

		public void DecodeQR()
		{
			while (Result == null)
			{
				// Wait
				if (Pixels == null || cameraWidth == 0 || cameraHeight == 0)
				{
					Thread.Sleep(100);
					continue;
				}

				// Process
				Debug.Log("SimpleScanner -> Scan ... " + cameraWidth + " / " + cameraHeight);
				try
				{
					Result = Parser.Decode(Pixels, cameraWidth, cameraHeight);
					Pixels = null;

					// Sleep a little bit and set the signal to get the next frame
					Thread.Sleep(250);
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
			}
		}

		

		public void Update()
		{
			if (Camera.IsReady() && Camera.IsPlaying())
			{
				//SetBestResolution();

				cameraWidth = Camera.Width;
				cameraHeight = Camera.Height;
				Pixels = Camera.GetPixels();

				if (Status == ScannerStatus.Initialize)
				{
					Status = ScannerStatus.Paused;
				}
			}
			else
			{
				Pixels = null;
			}

			if (Result != null)
			{
				Debug.LogWarning(Result);
				Status = ScannerStatus.Paused;

				// Call callback
				Callback(Result.Type, Result.Value);

				// Clean
				Callback = null;
				Result = null;
			}
		}

		/*
		private void SetBestResolution()
		{
			int maxDimension = 512;
			if (Camera.Width != 0 || Camera.WebcamSize.x < 100)
			{
				return;
			}

			float ratio = Camera.WebcamSize.x / Camera.WebcamSize.y;
			if (ratio > 1)
			{
				Camera.Width = maxDimension;
				Camera.Height = Mathf.RoundToInt((float)maxDimension / ratio);
			}
			else
			{
				Camera.Width = Mathf.RoundToInt((float)maxDimension * ratio);
				Camera.Height = maxDimension;
			}

			Camera.Stop();
			Camera.Play();
		}
		*/
	}
}
