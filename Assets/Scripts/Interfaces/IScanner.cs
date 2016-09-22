using BarcodeScanner.Scanner;
using System;

namespace BarcodeScanner
{
	public interface IScanner
	{
		event EventHandler StatusChanged;
		ScannerStatus Status { get; }

		IParser Parser { get; }
		IWebcam Camera { get; }

		void Scan(Action<string, string> Callback);
		void Stop();

		void Update();

	}
}
