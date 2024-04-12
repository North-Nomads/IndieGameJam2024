using System;

/// <summary>
/// Mob spawner class is responsible for instantiating mobs on level. Single entity per scene
/// </summary>
public class MobSpawner
{
    public readonly PlayerCombat Player;

    public event EventHandler<PlayerCombat> OnSpawnerLoaded = delegate { };
    public event EventHandler<int> OnMobDead = delegate { };
    public event EventHandler OnLevelCleared = delegate { };

    private int _mobsAliveLeft;
    public int MobAliveLeft => _mobsAliveLeft;

    public MobSpawner(PlayerCombat player, MobSpawnPoint[] spawnPoints)
    {
        Player = player;
        foreach (var spawnPoint in spawnPoints)
            spawnPoint.LinkMobSpawner(this);

        _mobsAliveLeft = spawnPoints.Length;
        OnSpawnerLoaded(this, Player);
    }

    public void HandleMobDeath(object sender, EventArgs e)
    {
        _mobsAliveLeft--;
        if (_mobsAliveLeft == 0)
            OnLevelCleared(this, null);
    }
}