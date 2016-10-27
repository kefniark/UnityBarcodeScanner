using System;
using System.Linq;
using UnityEngine;
using Wizcorp.Utils.Logger;

namespace BarcodeScanner.Webcam
{
	public class UnityWebcam : IWebcam
	{
		public event EventHandler OnInitialized;

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
		/// <param name="selectCamera"></param>
		public UnityWebcam(string selectCamera = "")
		{
			if (WebCamTexture.devices.Length == 0)
			{
				throw new Exception("No Camera on this device");
			}

			// Init Camera
			if (string.IsNullOrEmpty(selectCamera))
			{
				selectCamera = WebCamTexture.devices.First().name;
			}

			// Create Texture (512x512 is the max resolution, the camera will try to get the closer resolution possible)
			Webcam = new WebCamTexture(selectCamera);
			Webcam.requestedWidth = 512;
			Webcam.requestedHeight = 512;
			Webcam.filterMode = FilterMode.Trilinear;

			// Get size
			Width = 0;
			Height = 0;
		}

		public void SetSize()
		{
			Width = Mathf.RoundToInt(Webcam.width);
			Height = Mathf.RoundToInt(Webcam.height);
			Log.Info(string.Format("Camera {2} | Resolution {0}x{1}", Width, Height, Webcam.deviceName));
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
			int rotation = -Webcam.videoRotationAngle;
			if (Webcam.videoVerticallyMirrored)
			{
				rotation += 180;
			}
			return rotation;
		}

		public bool IsUpdatedThisFrame()
		{
			return Webcam.didUpdateThisFrame;
		}
	}
}
