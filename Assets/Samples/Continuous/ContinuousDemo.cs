using BarcodeScanner;
using BarcodeScanner.Scanner;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wizcorp.Utils.Logger;

public class ContinuousDemo : MonoBehaviour {

	private IScanner BarcodeScanner;
	public Text TextHeader;
	public RawImage Image;
	public AudioSource Audio;
	private float RestartTime;

	void Start () {
		// Create a basic scanner
		BarcodeScanner = new Scanner();
		BarcodeScanner.Camera.Play();

		// Display the camera texture through a RawImage
		BarcodeScanner.OnReady += (sender, arg) => {
			Log.Info("On Ready");
			Image.transform.localEulerAngles = new Vector3(0f, 0f, BarcodeScanner.Camera.GetRotation());
			Image.texture = BarcodeScanner.Camera.Texture;
			RestartTime = Time.realtimeSinceStartup;
		};
	}

	void Update()
	{
		if (BarcodeScanner != null)
		{
			BarcodeScanner.Update();
		}

		if (RestartTime != 0 && RestartTime < Time.realtimeSinceStartup)
		{
			// Check Scan
			BarcodeScanner.Scan((barCodeType, barCodeValue) => {
				BarcodeScanner.Stop();
				if (TextHeader.text.Length > 250)
				{
					TextHeader.text = "";
				}
				TextHeader.text += "Found: " + barCodeType + " / " + barCodeValue + "\n";
				RestartTime += Time.realtimeSinceStartup + 1f;
				Audio.Play();
			});
			RestartTime = 0;
		}
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

	public void ClickBack()
	{
		BarcodeScanner.Stop();
		SceneManager.LoadScene("Boot");
	}
}
