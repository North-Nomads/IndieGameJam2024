using System;
using UnityEngine;

/// <summary>
/// Observer is responsible for handling player actions
/// </summary>
public class LevelObserver
{
    private readonly RectTransform _deathPanel;
    private readonly RectTransform _levelFinishedPanel;

    public LevelObserver(RectTransform deathPanel, RectTransform levelFinishedPanel, PlayerHealth _player)
    {
        _deathPanel = deathPanel;
        _levelFinishedPanel = levelFinishedPanel;
        deathPanel.gameObject.SetActive(false);
        levelFinishedPanel.gameObject.SetActive(false);

        _player.OnPlayerDead += HandlePlayerDeath;
        _player.OnPlayerEscaped += HandleLevelFinished;
    }

    private void HandleLevelFinished(object sender, EventArgs e)
    {
        _levelFinishedPanel.gameObject.SetActive(true);
    }

    private void HandlePlayerDeath(object sender, EventArgs e)
    {
        _deathPanel.gameObject.SetActive(true);
    }
}
