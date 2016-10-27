using BarcodeScanner;
using BarcodeScanner.Scanner;
using BarcodeScanner.Webcam;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wizcorp.Utils.Logger;

public class SwitchDemo : MonoBehaviour {
	private UnityMultiWebcam MultiCamera;
	private IScanner BarcodeScanner;

	public Text TextHeader;
	public Text CameraName;
	public RawImage Image;
	public Image IconRecord;
	public AudioSource Audio;

	void Start () {

		MultiCamera = new UnityMultiWebcam();

		// Create a custom scanner
		BarcodeScanner = new Scanner(MultiCamera);

		// Display the camera texture through a RawImage
		BarcodeScanner.OnReady += (sender, arg) => {
			Image.transform.localEulerAngles = new Vector3(0f, 0f, BarcodeScanner.Camera.GetRotation());
			Image.texture = BarcodeScanner.Camera.Texture;

			CameraName.text = MultiCamera.SelectedCamera;
			TextHeader.text = "Tap to Scan";
			IconRecord.gameObject.SetActive(false);
		};
	}

	/// <summary>
	/// The Update method from unity need to be propagated to the scanner
	/// </summary>
	void Update()
	{
		if (BarcodeScanner == null)
		{
			return;
		}
		BarcodeScanner.Update();
	}

	#region UI Buttons

	public void ClickPhoto()
	{
		if (BarcodeScanner == null)
		{
			Log.Warning("No valid camera - Click Start");
			return;
		}

		//
		TextHeader.text = "";
		IconRecord.gameObject.SetActive(true);

		// Start Scanning
		BarcodeScanner.Scan((barCodeType, barCodeValue) => {
			TextHeader.text = "Found: " + barCodeType + " / " + barCodeValue;
			Audio.Play();

			BarcodeScanner.Stop();
			IconRecord.gameObject.SetActive(false);
		});
	}

	public void ClickSwitch()
	{
		if (BarcodeScanner == null)
		{
			Log.Warning("No valid camera - Click Stop");
			return;
		}

		// Clean
		BarcodeScanner.Stop(true);
		MultiCamera.CleanCurrentCamera();
		Image.texture = null;

		// Init next camera
		MultiCamera.SelectNextCamera();
	}

	public void ClickBack()
	{
		// Try to stop the camera before loading another scene
		StartCoroutine(StopCamera(() => {
			SceneManager.LoadScene("Boot");
		}));
	}

	/// <summary>
	/// This coroutine is used because of a bug with unity (http://forum.unity3d.com/threads/closing-scene-with-active-webcamtexture-crashes-on-android-solved.363566/)
	/// Trying to stop the camera in OnDestroy provoke random crash on Android
	/// </summary>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator StopCamera(Action callback)
	{
		// Stop Scanning
		Image = null;
		BarcodeScanner.Destroy();
		BarcodeScanner = null;

		// Wait a bit
		yield return new WaitForSeconds(0.1f);

		callback.Invoke();
	}

	#endregion
}
