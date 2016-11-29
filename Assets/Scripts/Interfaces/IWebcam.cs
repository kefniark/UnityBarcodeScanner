using UnityEngine;

namespace BarcodeScanner
{
	public interface IWebcam
	{
		// 
		Texture Texture { get; }
		int Width { get; }
		int Height { get; }

		//
		void SetSize();
		bool IsReady();
		bool IsPlaying();
		void Play();
		void Stop();
		void Destroy();

		//
		Color32[] GetPixels();
		float GetRotation();
		int GetChecksum();
	}
}