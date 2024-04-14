using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

/// <summary>
/// Observer is responsible for handling player actions
/// </summary>
public class LevelObserver
{
    private readonly LevelCanvas _deathPanel;
    private readonly LevelCanvas _levelFinishedPanel;
    private readonly LevelTimer _timer;


    public static event EventHandler OnLevelPaused = delegate { };
    public static bool IsLevelPaused { get; private set; }

    public LevelObserver(LevelCanvas deathPanel, LevelCanvas levelFinishedPanel, PlayerHealth _player, LevelTimer timer)
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
        _levelFinishedPanel.ShowCanvas(GenerateTitle(), _timer.TimeText);
        _timer.StopTimer();
    }

    private void HandlePlayerDeath(object sender, EventArgs e)
    {
        IsLevelPaused = true;
        _deathPanel.ShowCanvas(GenerateTitle(), _timer.TimeText);
        _timer.StopTimer();
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
