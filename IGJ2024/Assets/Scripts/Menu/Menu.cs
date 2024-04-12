using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private const string LevelPattern = "Level";

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene($"{LevelPattern}1");
    }
}