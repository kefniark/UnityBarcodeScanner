﻿using BarcodeScanner.Scanner;
using System;

namespace BarcodeScanner
{
	public interface IScanner
	{
		event EventHandler StatusChanged;
		event EventHandler OnReady;

		ScannerStatus Status { get; }

		IParser Parser { get; }
		IWebcam Camera { get; }

		void Scan(Action<string, string> Callback);
		void Stop(bool forced = false);
		void Update();
		void Destroy();

	}
}
