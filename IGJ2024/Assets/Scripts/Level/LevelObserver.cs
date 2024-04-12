using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Observer is responsible for handling player actions
/// </summary>
public class LevelObserver
{
    private readonly MobSpawner _mobSpawner;
    private readonly RectTransform _deathPanel;

    public LevelObserver(MobSpawner mobSpawner, RectTransform deathPanel, PlayerCombat _player)
    {
        _mobSpawner = mobSpawner;
        _mobSpawner.OnLevelCleared += HandleLevelClearance;
        _deathPanel = deathPanel;
        deathPanel.gameObject.SetActive(false);

        _player.OnPlayerDead += HandlePlayerDeath;
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

    private void HandlePlayerDeath(object sender, EventArgs e)
    {
        _deathPanel.gameObject.SetActive(true);
    }
}
