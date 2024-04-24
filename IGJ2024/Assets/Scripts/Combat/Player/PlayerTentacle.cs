using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerVFX))]
public class PlayerTentacle : MonoBehaviour
{
    public const float MaxSpeed = 16f;
    private const float TentacleSensitiveRadius = .5f;

    [Header("General")]
    [SerializeField] private float hookRange;
    [SerializeField] private LayerMask hookSurface;
    [SerializeField] private LayerMask possibleHookTargets;
    [SerializeField] private Transform hookPivot;
    [SerializeField] private float playerRidingHookSpeed;
    [SerializeField] private float hookSpeed;

    // GameObject components
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rigidbody;
    private PlayerVFX _playerVFX;
    private Animator _animator;
    private Camera _mainCamera;
    private LineRenderer _hookLineRender;
    
    // Fields
    private Coroutine _launchingTentacleProcess;
    private Vector2 _hookTarget;
    private bool _isHooking;
    private float _currentSpeed;
    private Coroutine _currentTentacleAnimationCoroutine;

    private Vector2 HookOrigin => new (hookPivot.position.x, hookPivot.position.y);
    private void UpdateHookLine(Vector2 endpoint) => _hookLineRender.SetPositions(new Vector3[] { HookOrigin, endpoint });
    private void HandlePlayerLanding(object sender, EventArgs e) => _currentSpeed = 0f;
    private bool IsTentacleTipSucked(Vector2 tentacleTipPoint) => Physics2D.OverlapCircle(tentacleTipPoint, TentacleSensitiveRadius, hookSurface.value);

    private void Start()
    {
        _mainCamera = Camera.main;
        _hookLineRender = GetComponent<LineRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerVFX = GetComponent<PlayerVFX>();
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerMovement.OnPlayerLanded += HandlePlayerLanding;
        LevelObserver.OnLevelPaused += HandleLevelPause;
    }    

    private void Update()
    {
        if (LevelObserver.IsLevelPaused)
            return;

        if (Input.GetMouseButtonDown(0))
            LaunchTentacle();

        if (Input.GetMouseButtonUp(0))
            ReleaseTentacle();
    }

    private void FixedUpdate()
    {
        if (LevelObserver.IsLevelPaused)
            return;

        if (_isHooking)
            RideHook();
    }

    private void HandleLevelPause(object sender, EventArgs e)
    {
        _animator.SetBool("IsHooking", false);
        _currentSpeed = 0f;
        _isHooking = false;
    }

    private void ReleaseTentacle()
    {
        HideTentacle();
        _animator.SetBool("IsHooking", false);
    }

    private void LaunchTentacle()
    {
        if (_currentTentacleAnimationCoroutine is not null)
            StopCoroutine(_currentTentacleAnimationCoroutine);

        // Turn towards the hook 
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _playerMovement.TurnTowards(mousePosition.x > HookOrigin.x);
        _hookLineRender.enabled = true;

        // Play launch animation and suck into hookable object
        HandleTentacleLaunchAnimatiomTowards(mousePosition);
    }

    private void HandleTentacleLaunchAnimatiomTowards(Vector2 mousePosition)
    {
        _currentTentacleAnimationCoroutine = StartCoroutine(PlayTentacleLaunchAnimation());
        
        IEnumerator PlayTentacleLaunchAnimation()
        {
            var direction = (mousePosition - HookOrigin).normalized;
            var maxTentacleDistance = direction * hookRange;

            var currentPoint = HookOrigin;
            var isGettingCloser = true;
            var previousDistance = Vector2.Distance(currentPoint, maxTentacleDistance);

            while (previousDistance > .1f && isGettingCloser)
            {
                currentPoint += direction * hookSpeed;
                isGettingCloser = Vector2.Distance(currentPoint, maxTentacleDistance) < previousDistance;
                previousDistance = Vector2.Distance(currentPoint, maxTentacleDistance);
                UpdateHookLine(currentPoint);

                if (IsTentacleTipSucked(currentPoint))
                {
                    HandleTentacleSuccessfullySuckedInto(currentPoint);
                    yield break;
                }

                yield return null;
            }

            HideTentacle();
        }
    }

    private void HandleTentacleSuccessfullySuckedInto(Vector2 targetPoint)
    {
        _playerVFX.SpawnHitVFX(_hookTarget);
        _playerVFX.AnimateCameraZoom(true);
        _animator.SetBool("IsHooking", true);
        _hookTarget = targetPoint;
        _isHooking = true;
    }

    private void HideTentacle()
    {
        if (_launchingTentacleProcess is not null)
            StopCoroutine(_launchingTentacleProcess);
        _hookLineRender.enabled = false;

        // If was hooking - zoom out. Otherwise - don't 
        if (_isHooking)
            _playerVFX.AnimateCameraZoom(false);

        // Reset variables for the next launch
        _isHooking = false;
    }

    private void RideHook()
    {
        _currentSpeed = playerRidingHookSpeed;
        Vector2 hookDirection = (_hookTarget - HookOrigin).normalized;
        _rigidbody.velocity = hookDirection * _currentSpeed;
        UpdateHookLine(_hookTarget);
    }
}
