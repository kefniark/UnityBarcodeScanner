using System;
using System.Linq;
using UnityEngine;
using Wizcorp.Utils.Logger;

namespace BarcodeScanner.Webcam
{
	public class UnityWebcam : IWebcam
	{
		public Texture Texture { get { return Webcam; } }
		public WebCamTexture Webcam { get; private set; }

		public Vector2 Size {
			get
			{
				return new Vector2(Webcam.width, Webcam.height);
			}
		}
		public int Width {get; private set; }
		public int Height  {get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public UnityWebcam()
		{
			if (WebCamTexture.devices.Length == 0)
			{
				throw new Exception("No Camera on this device");
			}

			// Init Camera
			WebCamDevice selectCamera = WebCamTexture.devices.First();
			Log.Info("Camera: " + selectCamera.name);

			// Create Texture
			Webcam = new WebCamTexture(selectCamera.name);
			Webcam.requestedWidth = 512;
			Webcam.requestedHeight = 512;
			//Webcam.filterMode = FilterMode.Trilinear;

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
			return Webcam != null && Webcam.width >= 100;
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
			Log.Info("Destroy Camera");
			if (Webcam.isPlaying)
			{
				Webcam.Stop();
			}
			GameObject.DestroyImmediate(Webcam, true);
		}

		public Color32[] GetPixels()
		{
			return Webcam.GetPixels32();
		}

		public float GetRotation()
		{
			int rotation = -Webcam.videoRotationAngle;
			if (Webcam.videoVerticallyMirrored)
			{
				rotation += 180;
			}
			return rotation;
		}
	}
}
