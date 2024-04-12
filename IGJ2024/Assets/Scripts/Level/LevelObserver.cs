using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Observer is responsible for handling player actions
/// </summary>
public class LevelObserver
{
    private readonly MobSpawner _mobSpawner;

    public LevelObserver(MobSpawner mobSpawner)
    {
        _mobSpawner = mobSpawner;
        _mobSpawner.OnLevelCleared += HandleLevelClearance;
    }

    private void HandleLevelClearance(object sender, EventArgs e)
    { 
        Debug.Log("Level cleared");
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings == nextSceneIndex)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(nextSceneIndex);
    }
}
