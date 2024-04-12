using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Observer is responsible for handling player actions
/// </summary>
public class LevelObserver
{
    private readonly RectTransform _deathPanel;

    public LevelObserver(RectTransform deathPanel, PlayerCombat _player)
    {
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
