using UnityEngine.SceneManagement;

public class SceneMaster : SingeltonPersistant<SceneMaster>
{
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
