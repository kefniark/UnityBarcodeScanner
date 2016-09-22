using UnityEngine;
using UnityEngine.UI;
using BarcodeScanner;
using BarcodeScanner.Scanner;

public class Demo : MonoBehaviour {

	private IScanner BarcodeScanner;
	private Text TextHeader;

	void Start () {
		// Create a basic scanner
		BarcodeScanner = new SimpleScanner();

		// Display the camera texture through a RawImage
		var rawImage = GameObject.Find("Canvas/RawImage").GetComponent<RawImage>();
		rawImage.texture = BarcodeScanner.Camera.Texture;

		TextHeader = GameObject.Find("Canvas/Text").GetComponent<Text>();

		// Start the 
		BarcodeScanner.Camera.Play();
		BarcodeScanner.StatusChanged += (sender, arg) => {
			TextHeader.text = "Status: " + BarcodeScanner.Status;
		};
	}

	void Update()
	{
		BarcodeScanner.Update();
	}

	void OnDestroy()
	{
		BarcodeScanner.Stop();
	}

	#region UI Buttons

	public void ClickStart()
	{
		BarcodeScanner.Scan((barCodeType, barCodeValue) => {
			Debug.LogWarning("Type: " + barCodeType);
			Debug.LogWarning("Value: " + barCodeValue);

			TextHeader.text = "Found: " + barCodeType + " / " + barCodeValue;
		});
	}

	public void ClickStop()
	{
		BarcodeScanner.Stop();
	}

	#endregion
}
