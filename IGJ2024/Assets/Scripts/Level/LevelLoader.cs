using UnityEngine;
using Cinemachine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private bool isTutorial = false;
    [SerializeField] private int tutorialIndex;
    
    private LevelObserver _levelObserver;
    private LevelTutorial _levelTutorial;
    private PlayerMovement _player;
    
    private LevelCanvas _deathPanel;
    private LevelCanvas _levelFinishPanel;
    private LevelTimer _clockInstante;

    private void Start()
    {
        SpawnPlayer();
        SpawnPlayerUI();
        SetUpPlayerCamera();
        _levelObserver = new LevelObserver(_deathPanel, _levelFinishPanel, _player.GetComponent<PlayerHealth>(), _clockInstante);

        if (isTutorial)
            _levelTutorial = new(tutorialIndex);
    }

    private void SetUpPlayerCamera()
    {
        var cinemachine = Instantiate(Resources.Load<GameObject>("Prefabs/Player/Cinemachine"));
        var cinemachineCamera = cinemachine.GetComponentInChildren<CinemachineVirtualCamera>();

        CompositeCollider2D environmentCollider = FindObjectOfType<CompositeCollider2D>();

        cinemachineCamera.Follow = _player.transform;
        cinemachineCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = environmentCollider;
        _player.GetComponent<PlayerVFX>().VirtualCamera = cinemachineCamera; 
    }

    private void SpawnPlayerUI()
    {
        var deathPanel = Resources.Load<LevelCanvas>("Prefabs/Player/DeathPanel");
        var levelFinishPanel = Resources.Load<LevelCanvas>("Prefabs/Player/LevelFinishPanel");
        
        var canvas = FindObjectOfType<Canvas>();
        _deathPanel = Instantiate(deathPanel, canvas.transform);
        _levelFinishPanel = Instantiate(levelFinishPanel, canvas.transform);

        var clockPanel = Resources.Load<LevelTimer>("Prefabs/Player/Clock");
        _clockInstante = Instantiate(clockPanel, canvas.transform);
        _clockInstante.StartTimer();

    }


    private void SpawnPlayer()
    {
        var playerPrefab = Resources.Load<PlayerMovement>("Prefabs/Player/Player");
        var playerSpawnPoint = GameObject.FindGameObjectWithTag("Player Spawn").transform.position;
        _player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
    }


}
