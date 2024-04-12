using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCanvas : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        var newSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (newSceneIndex == SceneManager.sceneCountInBuildSettings)
            ReturnToMenu();
        else
            SceneManager.LoadScene(newSceneIndex);

    }
}
