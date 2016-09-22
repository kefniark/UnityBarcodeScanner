using UnityEngine;

namespace BarcodeScanner
{
	public interface IWebcam
	{
		// 
		Texture Texture { get; }
		Vector2 WebcamSize { get; }
		int Width { get; set; }
		int Height { get; set; }

		//
		bool IsReady();
		bool IsPlaying();
		void Play();
		void Stop();

		//
		Color32[] GetPixels();
		float GetRotation();
	}
}