using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections;

/// <summary>
/// Observer is responsible for handling player actions
/// </summary>
public class LevelObserver
{
    private readonly LevelCanvas _deathPanel;
    private readonly LevelCanvas _levelFinishedPanel;
    private readonly PlayerHealth _player;
    private readonly LevelTimer _timer;
    private static bool _isLevelPaused;

    public static event EventHandler OnLevelPaused = delegate { };
    public static bool IsLevelPaused
    {
        get => _isLevelPaused;
        private set
        {
            _isLevelPaused = value;
            if (value)
                OnLevelPaused(null, null);
        }
    }


    public LevelObserver(LevelCanvas deathPanel, LevelCanvas levelFinishedPanel, PlayerHealth player, LevelTimer timer)
    {
        foreach (var sub in OnLevelPaused.GetInvocationList())
            OnLevelPaused -= sub as EventHandler;

        _deathPanel = deathPanel;
        _levelFinishedPanel = levelFinishedPanel;
        _player = player;
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
        _levelFinishedPanel.ShowCanvas(GenerateTitle(), _timer.TimeText);
        _timer.StopTimer();
        HidePlayer();
    }

    private void HandlePlayerDeath(object sender, EventArgs e)
    {
        IsLevelPaused = true;
        _deathPanel.ShowCanvas(GenerateTitle(), _timer.TimeText);
        _timer.StopTimer();
        HidePlayer();
    }

    private void HidePlayer()
    {
        _player.PlayFadeAnimation();
    }

    private string GenerateTitle()
    {
        var sceneName = SceneManager.GetActiveScene().name;

        string regexPattern = @"Level(\d+)";
        Match match = Regex.Match(sceneName, regexPattern);

        if (match.Success)
            return "Уровень " + match.Groups[1].Value;
        else
            return sceneName;
        

    }
}
