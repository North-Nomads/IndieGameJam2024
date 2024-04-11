using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private PlayerCombat player;

    private void Start()
    {
        SpawnPlayer();
        SpawnMobs();
    }

    private void SpawnMobs()
    {
        var spawnPoints = FindObjectsOfType<MobSpawnPoint>();
        var mobSpawner = new MobSpawner(player, spawnPoints);
    }

    private void SpawnPlayer()
    {
        var playerPrefab = Resources.Load<PlayerCombat>("Prefabs/Player/Player");
        var playerSpawnPoint = GameObject.FindGameObjectWithTag("Player Spawn").transform.position;
        player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
    }
}
