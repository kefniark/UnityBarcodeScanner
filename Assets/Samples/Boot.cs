using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
	public void OnClickSimple()
	{
		SceneManager.LoadScene("SimpleDemo");
	}

	public void OnContinuous()
	{
		SceneManager.LoadScene("ContinuousDemo");
	}
}
