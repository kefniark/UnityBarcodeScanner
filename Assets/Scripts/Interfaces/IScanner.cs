﻿using BarcodeScanner.Scanner;
using System;
using UnityEngine;

namespace BarcodeScanner
{
	public interface IScanner
	{
		event EventHandler StatusChanged;
		event EventHandler OnReady;

		ScannerStatus Status { get; }

		IParser Parser { get; }
		IWebcam Camera { get; }
		ScannerSettings Settings { get; }

		void Scan(Action<string, string> Callback);
		void Stop();
		void Update();
		void Destroy();
		Texture2D TakeScreenshot();
	}
}
