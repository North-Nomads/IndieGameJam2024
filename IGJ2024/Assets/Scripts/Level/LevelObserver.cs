using System;
using UnityEngine;

/// <summary>
/// Observer is responsible for handling player actions
/// </summary>
public class LevelObserver
{
    private readonly RectTransform _deathPanel;

    public LevelObserver(RectTransform deathPanel, PlayerMovement _player)
    {
        _deathPanel = deathPanel;
        deathPanel.gameObject.SetActive(false);

        _player.OnPlayerDead += HandlePlayerDeath;
    }

    private void HandlePlayerDeath(object sender, EventArgs e)
    {
        _deathPanel.gameObject.SetActive(true);
    }
}
