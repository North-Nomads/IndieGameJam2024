using System;
using UnityEngine;

/// <summary>
/// Mob spawner class is responsible for instantiating mobs on level. Single entity per scene
/// </summary>
public class MobSpawner
{
    public readonly PlayerCombat Player;

    public event EventHandler<PlayerCombat> OnSpawnerLoaded = delegate { };

    public MobSpawner(PlayerCombat player, MobSpawnPoint[] spawnPoints)
    {
        Player = player;
        foreach (var spawnPoint in spawnPoints)
            spawnPoint.LinkMobSpawner(this);
        
        OnSpawnerLoaded(this, Player);
    }
}