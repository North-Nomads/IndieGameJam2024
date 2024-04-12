using System;
using UnityEngine;

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
    }
}
