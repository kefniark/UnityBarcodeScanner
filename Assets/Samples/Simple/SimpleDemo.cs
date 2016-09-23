using UnityEngine;
using UnityEngine.UI;
using BarcodeScanner;
using BarcodeScanner.Scanner;
using Wizcorp.Utils.Logger;
using UnityEngine.SceneManagement;

public class SimpleDemo : MonoBehaviour {

	private IScanner BarcodeScanner;
	public Text TextHeader;
	public RawImage Image;
	public AudioSource Audio;

	void Start () {
		// Create a basic scanner
		BarcodeScanner = new Scanner();
		BarcodeScanner.Camera.Play();

		// Display the camera texture through a RawImage
		BarcodeScanner.OnReady += (sender, arg) => {
			Log.Info("On Ready");
			Image.transform.localEulerAngles = new Vector3(0f, 0f, BarcodeScanner.Camera.GetRotation());
			Image.texture = BarcodeScanner.Camera.Texture;
		};

		// Detect Match
		BarcodeScanner.StatusChanged += (sender, arg) => {
			TextHeader.text = "Status: " + BarcodeScanner.Status;
		};
	}

	void Update()
	{
		if (BarcodeScanner == null)
		{
			return;
		}
		BarcodeScanner.Update();
	}

	void OnDestroy()
	{
		if (BarcodeScanner == null)
		{
			Log.Warning("No valid camera - OnDestroy");
			return;
		}
		BarcodeScanner.Stop();
	}

	#region UI Buttons

	public void ClickStart()
	{
		if (BarcodeScanner == null)
		{
			Log.Warning("No valid camera - Click Start");
			return;
		}
		BarcodeScanner.Scan((barCodeType, barCodeValue) => {
			Log.Warning("Type: " + barCodeType);
			Log.Warning("Value: " + barCodeValue);

			TextHeader.text = "Found: " + barCodeType + " / " + barCodeValue;
			Audio.Play();
		});
	}

	public void ClickStop()
	{
		if (BarcodeScanner == null)
		{
			Log.Warning("No valid camera - Click Stop");
			return;
		}
		BarcodeScanner.Stop();
	}

	public void ClickBack()
	{
		SceneManager.LoadScene("Boot");
	}

	#endregion
}
