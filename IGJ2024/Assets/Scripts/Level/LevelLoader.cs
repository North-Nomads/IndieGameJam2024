using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private PlayerCombat _player;
    private MobSpawner _mobSpawner;
    private LevelObserver _levelObserver;

    private void Start()
    {
        SpawnPlayer();
        SpawnMobs();
        _levelObserver = new LevelObserver(_mobSpawner);
    }

    private void SpawnMobs()
    {
        var spawnPoints = FindObjectsOfType<MobSpawnPoint>();
        _mobSpawner = new MobSpawner(_player, spawnPoints);
    }

    private void SpawnPlayer()
    {
        var playerPrefab = Resources.Load<PlayerCombat>("Prefabs/Player/Player");
        var playerSpawnPoint = GameObject.FindGameObjectWithTag("Player Spawn").transform.position;
        _player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
    }
}
