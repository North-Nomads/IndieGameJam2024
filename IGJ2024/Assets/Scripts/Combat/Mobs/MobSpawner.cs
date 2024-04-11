using System;
using UnityEngine;

/// <summary>
/// Mob spawner class is responsible for instantiating mobs on level. Single entity per scene
/// </summary>
public class MobSpawner
{
    public readonly Transform Player;

    public event EventHandler<Transform> OnSpawnerLoaded = delegate { };

    public MobSpawner(Transform player, MobSpawnPoint[] spawnPoints)
    {
        Player = player;
        foreach (var spawnPoint in spawnPoints)
            spawnPoint.LinkMobSpawner(this);
        
        OnSpawnerLoaded(this, Player);
    }
}