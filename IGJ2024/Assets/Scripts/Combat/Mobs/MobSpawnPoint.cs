using UnityEngine;

/// <summary>
/// An instance of spawn point that is placed on map and represents a spot where to spawn a mob
/// Dependent on MobSpawner as an event subscriber 
/// </summary>
public class MobSpawnPoint : MonoBehaviour
{
    [SerializeField] private MobBehaviour mobToSpawn;
    private MobSpawner _spawner;

    private void Start()
    {
        _spawner = FindObjectOfType<MobSpawner>(); // rewrite smh
        _spawner.OnPlayerLoaded += SpawnMob;
    }

    private void SpawnMob(object sender, Transform e)
    {
        var mob = Instantiate(mobToSpawn, transform.position, Quaternion.identity);
        mob.PlayerInstance = e;

    }
}