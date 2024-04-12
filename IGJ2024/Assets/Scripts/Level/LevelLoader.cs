using System;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private PlayerMovement _player;
    private RectTransform _deathPanel;
    private LevelObserver _levelObserver;

    private void Start()
    {
        SpawnPlayer();
        SpawnPlayerUI();
        _levelObserver = new LevelObserver(_deathPanel, _player);
    }

    private void SpawnPlayerUI()
    {
        var deathPanel = Resources.Load<RectTransform>("Prefabs/Player/DeathPanel");
        var canvas = FindObjectOfType<Canvas>();
        _deathPanel = Instantiate(deathPanel, canvas.transform);
    }


    private void SpawnPlayer()
    {
        var playerPrefab = Resources.Load<PlayerMovement>("Prefabs/Player/Player");
        var playerSpawnPoint = GameObject.FindGameObjectWithTag("Player Spawn").transform.position;
        _player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
    }
}
