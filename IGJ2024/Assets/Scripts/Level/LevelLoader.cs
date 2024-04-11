using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private PlayerMovement player;

    private void Start()
    {
        SpawnPlayer();
        SpawnMobs();
    }

    private void SpawnMobs()
    {
        var spawnPoints = FindObjectsOfType<MobSpawnPoint>();
        var mobSpawner = new MobSpawner(player.transform, spawnPoints);
    }

    private void SpawnPlayer()
    {
        var playerPrefab = Resources.Load<PlayerMovement>("Prefabs/Player/Player");
        var playerSpawnPoint = GameObject.FindGameObjectWithTag("Player Spawn").transform.position;
        player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
    }
}
