using UnityEngine;

/// <summary>
/// An instance of spawn point that is placed on map and represents a spot where to spawn a mob
/// Dependent on MobSpawner as an event subscriber 
/// </summary>
public class MobSpawnPoint : MonoBehaviour
{
    [SerializeField] private MobBehaviour mobToSpawn;
    private MobSpawner _mobSpawner;

    public void LinkMobSpawner(MobSpawner spawner)
    {
        spawner.OnSpawnerLoaded += SpawnMob;
        _mobSpawner = spawner;
    }

    private void SpawnMob(object sender, PlayerCombat e)
    {
        var mob = Instantiate(mobToSpawn, transform.position, Quaternion.identity);
        mob.PlayerInstance = e;
        mob.OnMobDeath += _mobSpawner.HandleMobDeath;
    }
}