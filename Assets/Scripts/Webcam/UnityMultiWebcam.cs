using System;
using UnityEngine;
using Wizcorp.Utils.Logger;

namespace BarcodeScanner.Webcam
{
	public class UnityMultiWebcam : IWebcam
	{
		public event EventHandler OnInitialized;

		public Texture Texture { get { return CurrentCamera.Texture; } }
		public int Width { get { return CurrentCamera.Width; } }
		public int Height { get { return CurrentCamera.Height; } }

		public IWebcam CurrentCamera { get; private set; }
		public string SelectedCamera { get; private set; }

		public UnityMultiWebcam()
		{
			if (WebCamTexture.devices.Length == 0)
			{
				throw new Exception("No Camera on this device");
			}

			// Init Camera
			SelectNextCamera();
		}

		#region Switch Camera

		/// <summary>
		/// 
		/// </summary>
		public void SelectNextCamera()
		{
			int index = 0;
			var devices = WebCamTexture.devices;

			if (!string.IsNullOrEmpty(SelectedCamera))
			{
				// Find the previous device
				for (int i = 0; i < devices.Length; i++)
				{
					if (devices[i].name == SelectedCamera)
					{
						index = i + 1;
					}
				}

				// 
				if (index >= devices.Length)
				{
					index = 0;
				}
			}
			
			// Select the camera
			SelectCamera(devices[index].name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public void SelectCamera(string name)
		{
			if (CurrentCamera != null)
			{
				CleanCurrentCamera();
			}

			Log.Info("SelectCamera " + name);

			// Init Webcam
			CurrentCamera = new UnityWebcam(name);
			SelectedCamera = name;

			// Player Camera
			CurrentCamera.Play();

			//
			if (OnInitialized != null)
			{
				OnInitialized.Invoke(this, EventArgs.Empty);
			}
		}

		#endregion

		public void CleanCurrentCamera()
		{
			CurrentCamera.Destroy();
			CurrentCamera = null;
		}

		public void Destroy()
		{
			CurrentCamera.Destroy();
		}

		public Color32[] GetPixels()
		{
			if (CurrentCamera == null)
			{
				return null;
			}
			return CurrentCamera.GetPixels();
		}

		public float GetRotation()
		{
			if (CurrentCamera == null)
			{
				return 0;
			}
			return CurrentCamera.GetRotation();
		}

		public bool IsPlaying()
		{
			if (CurrentCamera == null)
			{
				return false;
			}
			return CurrentCamera.IsPlaying();
		}

		public bool IsReady()
		{
			if (CurrentCamera == null)
			{
				return false;
			}
			return CurrentCamera.IsReady();
		}

		public void Play()
		{
			CurrentCamera.Play();
		}

		public void SetSize()
		{
			CurrentCamera.SetSize();
		}

		public void Stop()
		{
			CurrentCamera.Stop();
		}

		public bool IsUpdatedThisFrame()
		{
			if (CurrentCamera == null)
			{
				return false;
			}
			return CurrentCamera.IsUpdatedThisFrame();
		}
	}
}
