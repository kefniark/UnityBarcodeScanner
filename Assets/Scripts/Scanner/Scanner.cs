using BarcodeScanner.Parser;
using BarcodeScanner.Webcam;
using System;
using UnityEngine;
using Wizcorp.Utils.Logger;

#if !UNITY_WEBGL
using System.Threading;
#endif

namespace BarcodeScanner.Scanner
{
	/// <summary>
	/// This Scanner Is used to manage the Camera & the parser and provide:
	/// * Simple methods : Scan / Stop
	/// * Simple events : OnStatus / StatusChanged
	/// </summary>
	public class Scanner : IScanner
	{
		//
		public event EventHandler OnReady;
		public event EventHandler StatusChanged;

		//
		public IWebcam Camera { get; private set; }
		public IParser Parser { get; private set; }

		//
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

		// Allow to switch between decoding method (some platform don't support background thread)
		private bool useBackgroundThread = true;
		private float mainThreadLastDecode = 0;

		// Store information about last image / results (use the update loop to access camera and callback)
		private Color32[] pixels = new Color32[0];
		private Action<string, string> Callback;
		private ParserResult Result;

		//
		private int delayFrameWebcamMin = 3;
		private int delayFrameWebcam = 0;
		private int lastCRC = -1;

		public Scanner(IParser parser = null, IWebcam webcam = null)
		{
			// Check Device Authorization
			if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
			{
				throw new Exception("This Webcam Library can't work without the webcam authorization");
			}

			Status = ScannerStatus.Initialize;
			Parser = (parser == null) ? new ZXingParser() : parser;
			Camera = (webcam == null) ? new UnityWebcam() : webcam;

			#if UNITY_WEBGL
			useBackgroundThread = false;
			#endif
		}

		/// <summary>
		/// Used to start Scanning
		/// </summary>
		/// <param name="callback"></param>
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

			#if !UNITY_WEBGL
			if (useBackgroundThread)
			{
				CodeScannerThread = new Thread(ThreadDecodeQR);
				CodeScannerThread.Start();
			}
			#endif
		}

		/// <summary>
		/// Used to Stop Scanning
		/// </summary>
		public void Stop()
		{
			Stop(false);
		}

		/// <summary>
		/// Used to Stop Scanning internaly (can be forced)
		/// </summary>
		private void Stop(bool forced)
		{
			if (!forced && Callback == null)
			{
				Log.Warning("No Scan running");
				return;
			}

			// Stop thread / Clean callback
			Log.Info("SimpleScanner -> Stop Scan");
			#if !UNITY_WEBGL
			if (CodeScannerThread != null)
			{
				CodeScannerThread.Abort();
			}
			#endif
			Callback = null;
			Status = ScannerStatus.Paused;
		}

		/// <summary>
		/// Used to be sure that everything is properly clean
		/// </summary>
		public void Destroy()
		{
			// clean events
			OnReady = null;
			StatusChanged = null;

			// Stop it
			Stop(true);

			// clean returns
			Callback = null;
			Result = null;
			pixels = null;

			// clean camera
			Camera.Destroy();
			Camera = null;
			Parser = null;
		}

		#region Unthread

		/// <summary>
		/// Process Image Decoding in the main Thread
		/// Background Thread : OFF
		/// </summary>
		public void DecodeQR()
		{
			// Wait
			if (Status != ScannerStatus.Running || pixels.Length == 00 || Camera.Width == 0)
			{
				return;
			}

			// Process
			Log.Debug("SimpleScanner -> Scan ... " + Camera.Width + " / " + Camera.Height);
			try
			{
				Result = Parser.Decode(pixels, Camera.Width, Camera.Height);
				pixels = null;
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		#endregion

		#region Background Thread

		#if !UNITY_WEBGL
		private Thread CodeScannerThread;

		/// <summary>
		/// Process Image Decoding in a Background Thread
		/// Background Thread : OFF
		/// </summary>
		public void ThreadDecodeQR()
		{
			while (Result == null)
			{
				if (pixels == null) {
					pixels = new Color32[0];
				}

				// Wait
				if (Status != ScannerStatus.Running || pixels.Length == 00 || Camera.Width == 0)
				{
					Thread.Sleep(100);
					continue;
				}

				// Process
				Log.Debug("SimpleScanner -> Scan ... " + Camera.Width + " / " + Camera.Height);
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
		#endif

		#endregion

		/// <summary>
		/// This Update Loop is used to :
		/// * Wait the Camera is really ready
		/// * Bring back Callback to the main thread when using Background Thread
		/// * To execute image Decoding When not using the background Thread
		/// </summary>
		public void Update()
		{
			// If not ready, wait
			if (!Camera.IsReady())
			{
				Log.Warning("Camera Not Ready Yet ...");
				if (status != ScannerStatus.Initialize)
				{
					Status = ScannerStatus.Initialize;
				}
				return;
			}

			// Be sure that the camera metadata is stable (thanks Unity)
			if (lastCRC != Camera.GetCRC())
			{
				lastCRC = Camera.GetCRC();
				delayFrameWebcam = 0;
				Log.Info(string.Format("Camera [Resolution:{0}x{1}, Orientation:{2}, IsPlaying:{3}]", Camera.Texture.width, Camera.Texture.height, Camera.GetRotation(), Camera.IsPlaying()));
				return;
			}

			// If the app start for the first time (select size & onReady Event)
			if (Status == ScannerStatus.Initialize)
			{
				if (delayFrameWebcam < delayFrameWebcamMin)
				{
					delayFrameWebcam++;
					return;
				}

				Status = ScannerStatus.Paused;

				Camera.SetSize();
				delayFrameWebcam = 0;

				if (OnReady != null)
				{
					OnReady.Invoke(this, EventArgs.Empty);
				}
			}

			if (Status == ScannerStatus.Running)
			{
				// Call the callback if a result is there
				if (Result != null)
				{
					//
					Log.Info(Result);
					Callback(Result.Type, Result.Value);

					// clean and return
					Result = null;
					pixels = new Color32[0];
					return;
				}

				// Get the image as an array of Color32
				pixels = Camera.GetPixels();

				// If background thread OFF, do the decode main thread with 500ms of pause for UI
				if (!useBackgroundThread && mainThreadLastDecode < Time.realtimeSinceStartup - 0.5f)
				{
					DecodeQR();
					mainThreadLastDecode = Time.realtimeSinceStartup;
				}
			}
		}
	}
}
