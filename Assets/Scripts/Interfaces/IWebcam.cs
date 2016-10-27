using System;
using UnityEngine;

namespace BarcodeScanner
{
	public interface IWebcam
	{
		event EventHandler OnInitialized;

		// 
		Texture Texture { get; }
		int Width { get; }
		int Height { get; }

		//
		void SetSize();
		bool IsReady();
		bool IsPlaying();
		bool IsUpdatedThisFrame();

		void Play();
		void Stop();
		void Destroy();

		//
		Color32[] GetPixels();
		float GetRotation();
	}
}