using System;
using UnityEngine;

/// <summary>
/// Observer is responsible for handling player actions
/// </summary>
public class LevelObserver
{
    private readonly RectTransform _deathPanel;
    private readonly RectTransform _levelFinishedPanel;
    private readonly LevelTimer _timer;


    public static event EventHandler OnLevelPaused = delegate { };
    public static bool IsLevelPaused { get; private set; }

    public LevelObserver(RectTransform deathPanel, RectTransform levelFinishedPanel, PlayerHealth _player, LevelTimer timer)
    {
        _deathPanel = deathPanel;
        _levelFinishedPanel = levelFinishedPanel;
        _timer = timer;
        deathPanel.gameObject.SetActive(false);
        levelFinishedPanel.gameObject.SetActive(false);

        _player.OnPlayerDead += HandlePlayerDeath;
        _player.OnPlayerEscaped += HandleLevelFinished;
        IsLevelPaused = false;
    }

    private void HandleLevelFinished(object sender, EventArgs e)
    {
        IsLevelPaused = true;
        _levelFinishedPanel.gameObject.SetActive(true);
        _timer.StopTimer();
    }

    private void HandlePlayerDeath(object sender, EventArgs e)
    {
        IsLevelPaused = true;
        _deathPanel.gameObject.SetActive(true);
        _timer.StopTimer();
    }
}
