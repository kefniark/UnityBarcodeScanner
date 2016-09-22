using System;
using UnityEngine;

namespace BarcodeScanner.Webcam
{
	public class UnityWebcam : IWebcam
	{
		public Texture Texture { get { return Webcam; } }
		public WebCamTexture Webcam { get; private set; }

		public Vector2 WebcamSize {
			get
			{
				return new Vector2(Webcam.width, Webcam.height);
			}
		}

		public int Width
		{
			get
			{
				return Webcam.requestedWidth;
			}
			set
			{
				Webcam.requestedWidth = value;
			}
		}

		public int Height
		{
			get
			{
				return Webcam.requestedHeight;
			}
			set
			{
				Webcam.requestedHeight = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public UnityWebcam()
		{
			Webcam = new WebCamTexture();
		}

		public bool IsReady()
		{
			return Webcam != null;
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

		public Color32[] GetPixels()
		{
			return Webcam.GetPixels32();
		}

		public float GetRotation()
		{
			return Webcam.videoRotationAngle;
		}
	}
}
