using System;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private PlayerCombat _player;
    private MobSpawner _mobSpawner;
    private RectTransform _deathPanel;
    private LevelObserver _levelObserver;

    private void Start()
    {
        SpawnPlayer();
        SpawnPlayerUI();
        SpawnMobs();
        _levelObserver = new LevelObserver(_mobSpawner, _deathPanel, _player);
    }

    private void SpawnPlayerUI()
    {
        var deathPanel = Resources.Load<RectTransform>("Prefabs/Player/DeathPanel");
        var canvas = FindObjectOfType<Canvas>();
        _deathPanel = Instantiate(deathPanel, canvas.transform);
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
