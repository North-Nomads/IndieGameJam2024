using UnityEngine;
using Cinemachine;

public class LevelLoader : MonoBehaviour
{
    private PlayerMovement _player;
    private RectTransform _deathPanel;
    private LevelObserver _levelObserver;
    private RectTransform _levelFinishPanel;

    private void Start()
    {
        SpawnPlayer();
        SpawnPlayerUI();
        SetUpPlayerCamera();
        _levelObserver = new LevelObserver(_deathPanel, _levelFinishPanel, _player);
    }

    private void SetUpPlayerCamera()
    {
        var cinemachine = Instantiate(Resources.Load<GameObject>("Prefabs/Player/Cinemachine"));
        var cinemachineCamera = cinemachine.GetComponentInChildren<CinemachineVirtualCamera>();

        CompositeCollider2D environmentCollider = FindObjectOfType<CompositeCollider2D>();

        cinemachineCamera.Follow = _player.transform;
        cinemachineCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = environmentCollider;
    }

    private void SpawnPlayerUI()
    {
        var deathPanel = Resources.Load<RectTransform>("Prefabs/Player/DeathPanel");
        var levelFinishPanel = Resources.Load<RectTransform>("Prefabs/Player/LevelFinishPanel");
        
        var canvas = FindObjectOfType<Canvas>();
        _deathPanel = Instantiate(deathPanel, canvas.transform);
        _levelFinishPanel = Instantiate(levelFinishPanel, canvas.transform);
    }


    private void SpawnPlayer()
    {
        var playerPrefab = Resources.Load<PlayerMovement>("Prefabs/Player/Player");
        var playerSpawnPoint = GameObject.FindGameObjectWithTag("Player Spawn").transform.position;
        _player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
    }


}
