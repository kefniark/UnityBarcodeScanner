using UnityEngine;
using Wizcorp.Utils.Logger;

namespace BarcodeScanner.Webcam
{
	public class UnityWebcam : IWebcam
	{
		public Texture Texture { get { return Webcam; } }
		public WebCamTexture Webcam { get; private set; }

		public Vector2 Size { get { return new Vector2(Webcam.width, Webcam.height); } }
		public int Width { get; private set; }
		public int Height { get; private set; }

		public UnityWebcam() : this(new ScannerSettings())
		{
		}

		public UnityWebcam(ScannerSettings settings)
		{
			// Create Webcam Texture
			Webcam = new WebCamTexture(settings.WebcamDefaultDeviceName);
			Webcam.requestedWidth = settings.WebcamRequestedWidth;
			Webcam.requestedHeight = settings.WebcamRequestedHeight;
			Webcam.filterMode = settings.WebcamFilterMode;

			// Get size
			Width = 0;
			Height = 0;
		}

		public void SetSize()
		{
			Width = Mathf.RoundToInt(Webcam.width);
			Height = Mathf.RoundToInt(Webcam.height);
		}

		public bool IsReady()
		{
			return Webcam != null && Webcam.width >= 100 && Webcam.videoRotationAngle % 90 == 0;
		}

		public bool IsPlaying()
		{
			return Webcam.isPlaying;
		}

		public void Play()
		{
			Webcam.Play();
		}

		public void Stop()
		{
			Webcam.Stop();
		}

		public void Destroy()
		{
			if (Webcam.isPlaying)
			{
				Webcam.Stop();
			}
		}

		public Color32[] GetPixels()
		{
			return Webcam.GetPixels32();
		}

		public float GetRotation()
		{
			return -Webcam.videoRotationAngle;
		}

		public bool IsVerticalyMirrored()
		{
			return Webcam.videoVerticallyMirrored;
		}

		public int GetChecksum()
		{
			return (Webcam.width + Webcam.height + Webcam.deviceName + Webcam.videoRotationAngle).GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[UnityWebcam] Camera: {2} | Resolution: {0}x{1} | Orientation: {3}", Width, Height, Webcam.deviceName, Webcam.videoRotationAngle + ":" + Webcam.videoVerticallyMirrored);
		}
	}
}
