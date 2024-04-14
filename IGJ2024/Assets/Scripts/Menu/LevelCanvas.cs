using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI speedText;

    public void ShowCanvas(string title, string time)
    {
        gameObject.SetActive(true);
        speedText.text = time;
        titleText.text = title;
    }

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
