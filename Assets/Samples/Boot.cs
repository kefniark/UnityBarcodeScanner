using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is just a boot screen use to start the app
/// </summary>
public class Boot : MonoBehaviour
{
	void Awake()
	{
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = true;

		// Enable vsync for the samples (avoid running mobile device at 300fps)
		QualitySettings.vSyncCount = 1;
	}

	IEnumerator Start()
	{
		// When the app start, ask for the authorization to use the webcam
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

		if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
		{
			throw new Exception("This Webcam library can't work without the webcam authorization");
		}
	}

	#region UI Buttons

	public void OnClickSimple()
	{
		SceneManager.LoadScene("SimpleDemo");
	}

	public void OnContinuous()
	{
		SceneManager.LoadScene("ContinuousDemo");
	}

	#endregion
}