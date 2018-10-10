using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneChange : MonoBehaviour
{
	private VideoPlayer videoPlayer;
    private bool isLoadingScene = false;

	private void SceneSwitch()
	{
        if (!isLoadingScene)
        {
            isLoadingScene = true;
            SceneManager.LoadScene(1);
        }
	}

	private void Awake ()
	{
		videoPlayer = GetComponent<VideoPlayer>();
	}

	private void Start()
	{
		Screen.SetResolution(1920, 1080, true);
		StartCoroutine(IPlayIntroScene());
	}

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Space))
        {
            SceneSwitch();
        }
    }

    public void SetGraphicValues()
	{
		Screen.SetResolution(1920, 1080, true);
	}

	private IEnumerator IPlayIntroScene()
	{
		videoPlayer.Play();

		yield return new WaitUntil(() => !videoPlayer.isPlaying);

		SceneSwitch();
	}
}
