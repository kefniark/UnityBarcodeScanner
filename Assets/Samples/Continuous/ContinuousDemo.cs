using BarcodeScanner;
using BarcodeScanner.Scanner;
using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using System.Linq;

public class ContinuousDemo : MonoBehaviour {

	private IScanner BarcodeScanner;
	public Text TextHeader;
	public RawImage Image;
	public AudioSource Audio;
	private float RestartTime;

	public int resWidth = Screen.width;
        public int resHeight = Screen.height;

	// Disable Screen Rotation on that screen
	void Awake()
	{
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
	}

	void Start () {
		//QualitySettings.SetQualityLevel(1, false);
		
		// Create a basic scanner
		BarcodeScanner = new Scanner();
		BarcodeScanner.Camera.Play();

		// Display the camera texture through a RawImage
		BarcodeScanner.OnReady += (sender, arg) => {
			// Set Orientation & Texture
			Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
			Image.transform.localScale = BarcodeScanner.Camera.GetScale();
			Image.texture = BarcodeScanner.Camera.Texture;

			// Keep Image Aspect Ratio
			var rect = Image.GetComponent<RectTransform>();
			var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
			rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);

			RestartTime = Time.realtimeSinceStartup;
		};
	}

	/// <summary>
	/// Start a scan and wait for the callback (wait 1s after a scan success to avoid scanning multiple time the same element)
	/// </summary>
	private void StartScanner()
	{
		BarcodeScanner.Scan((barCodeType, barCodeValue) => {
			BarcodeScanner.Stop();

			// wait 2 seconds till next scan
			RestartTime += Time.realtimeSinceStartup + 2f;

			// Feedback
			Audio.Play();

			#if UNITY_ANDROID || UNITY_IOS
			Handheld.Vibrate();
			#endif

			RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
			Camera camera = this.GetComponent<Camera>();
                        camera.targetTexture = rt;
                        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
                        camera.Render();
                        RenderTexture.active = rt;
                        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
                        camera.targetTexture = null;
                        RenderTexture.active = null; // JC: added to avoid errors
                        Destroy(rt);
            
                        string time = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
			string folderName = string.Format("{0}/captures", 
                            				Application.persistentDataPath);
			string screenshotFileName = string.Format("{0}/screen_{1}x{2}_{3}.png", 
                            				folderName, 
                            				resWidth, resHeight, 
                            				time);
			string dataFileName = string.Format("{0}/data_{1}.txt", 
                            		folderName, 
                           			time);
			if(!Directory.Exists(folderName)){
				Directory.CreateDirectory(folderName);
			}

		 	System.IO.File.WriteAllBytes(screenshotFileName, BarcodeScanner.TakeScreenshot().EncodeToPNG());
			System.IO.File.WriteAllBytes(dataFileName, Encoding.ASCII.GetBytes("Found: " + barCodeType + " / " + barCodeValue + "\n"));

			if (TextHeader.text.Length > 250)
			{
				TextHeader.text = "";
			}
			TextHeader.text += "Filename " + screenshotFileName + "\n";
		});
	}

	/// <summary>
	/// The Update method from unity need to be propagated
	/// </summary>
	void Update()
	{
		if (BarcodeScanner != null)
		{
			BarcodeScanner.Update();
		}

		// Check if the Scanner need to be started or restarted
		if (RestartTime != 0 && RestartTime < Time.realtimeSinceStartup)
		{
			StartScanner();
			RestartTime = 0;
		}
	}

	// this #region thing is real code, don't delete it
	#region UI Buttons

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
