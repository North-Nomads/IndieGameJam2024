using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerVFX))]
public class PlayerTentacle : MonoBehaviour
{
    public const float MaxSpeed = 16f;

    [Header("General")]
    [SerializeField] private float hookRange;
    [SerializeField] private LayerMask hookSurface;
    [SerializeField] private LayerMask possibleHookTargets;
    [SerializeField] private Transform hookPivot;

    [Header("Speed")]
    [SerializeField] private AnimationCurve hookSpeed;
    [SerializeField] private float hookSpeedMultiplier;
    [SerializeField] private float hookGrowthSpeed;
    [SerializeField] private float hookBoostTime;

    // GameObject components
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rigidbody;
    private PlayerVFX _playerVFX;
    private Animator _animator;
    private Camera _mainCamera;
    private LineRenderer _line;
    
    // Fields
    private Coroutine _launchingTentacleProcess;
    private float _hookMountElapsedTime;
    private Vector2 _hookOrigin;
    private Vector2 _hookTarget;
    private bool _isHooking;
    private float _currentSpeed;
    private float _windSpeed;

    private bool HasReachedHookTarget => Vector2.Distance(_hookTarget, _hookOrigin) < 1f;


    private void Start()
    {
        _mainCamera = Camera.main;
        _line = GetComponent<LineRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerVFX = GetComponent<PlayerVFX>();
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerMovement.OnPlayerLanded += HandlePlayerLanding;

        _windSpeed = hookSpeed.Evaluate(1) * .05f * Time.fixedDeltaTime;

        LevelObserver.OnLevelPaused += HandleLevelPause;
    }

    private void HandleLevelPause(object sender, EventArgs e)
    {
        _animator.SetBool("IsHooking", false);
        _currentSpeed = 0f;
        _isHooking = false;
    }

    private void HandlePlayerLanding(object sender, EventArgs e)
    {
        _currentSpeed = 0f;
    }

    private void Update()
    {
        if (LevelObserver.IsLevelPaused)
            return;

        _animator.SetBool("IsHooking", _isHooking);
        _hookOrigin = new (hookPivot.position.x, hookPivot.position.y);

        if (Input.GetMouseButtonDown(0))
            LaunchTentacle();

        if (Input.GetMouseButtonUp(0))
            ReleaseTentacle();
    }

    private void ReleaseTentacle()
    {
        if (_launchingTentacleProcess is not null)
            StopCoroutine(_launchingTentacleProcess);

        // If was hooking - zoom out. Otherwise - don't 
        if (_isHooking)
            _playerVFX.AnimateCameraZoom(false);

        // Reset variables for the next launch
        _isHooking = false;
        ClearHookLine();
        _hookMountElapsedTime = 0f;
    }

    private void LaunchTentacle()
    {
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(_hookOrigin, mousePosition - _hookOrigin, hookRange, possibleHookTargets.value);

        _playerMovement.TurnTowards(mousePosition.x > _hookOrigin.x);

        ProcessLaunchResult(mousePosition, hit, out bool wasClickMiss, out bool wasHitSuccessful);

        EnableHook();
        _launchingTentacleProcess = StartCoroutine(LaunchTentacle(wasHitSuccessful, !wasClickMiss));

        void ProcessLaunchResult(Vector2 mousePosition, RaycastHit2D hit, out bool hasMissed, out bool isHitSuccessful)
        {
            hasMissed = hit.collider == null;
            isHitSuccessful = false;

            // By default, player has missed.
            // The tentacle will try to reach the point in that direction by the tentacle range
            _hookTarget = _rigidbody.position + (mousePosition - _rigidbody.position).normalized * hookRange;

            // In case if player has hit something
            if (!hasMissed)
            {
                // In case if the player hit the pipes - he successfully sucks on it
                isHitSuccessful = Physics2D.OverlapCircle(hit.point, .1f, hookSurface.value);
                _hookTarget = hit.point; // the point will be overwritten in any case
            }
        }
    }

    private IEnumerator LaunchTentacle(bool hasHit, bool shouldSpawnVFX)
    {
        var direction = (_hookOrigin - _hookTarget).normalized;

        var currentPoint = _hookOrigin;
        var isGettingCloser = true;
        var previousDistance = Vector2.Distance(currentPoint, _hookTarget);

        while (previousDistance > .1f && isGettingCloser)
        {
            currentPoint -= direction * hookGrowthSpeed;
            isGettingCloser = Vector2.Distance(currentPoint, _hookTarget) < previousDistance;
            previousDistance = Vector2.Distance(currentPoint, _hookTarget); 
            UpdateHookLine(currentPoint);
            yield return null;
        }
        
        _isHooking = hasHit;
        if (shouldSpawnVFX)
            _playerVFX.SpawnHitVFX(_hookTarget);

        if (_isHooking)
            _playerVFX.AnimateCameraZoom(true);
    }

    private void FixedUpdate()
    {
        if (LevelObserver.IsLevelPaused)
            return;

        if (_isHooking)
            RideHook();
    }

    private void RideHook()
    {
        _currentSpeed = EvaluateSpeed();

        Vector2 hookDirection = (_hookTarget - _hookOrigin).normalized;
        _rigidbody.velocity = hookDirection * _currentSpeed;
        UpdateHookLine(_hookTarget);

        float EvaluateSpeed()
        {
            // During the boost time -> increase the speed
            // Once the boosting time is over - maintain end point speed

            _hookMountElapsedTime += Time.fixedDeltaTime;
            float clamp = Mathf.Clamp01(_hookMountElapsedTime / hookBoostTime);
            float newSpeed;
            if (clamp < 1)
                newSpeed = _currentSpeed + hookSpeed.Evaluate(clamp) * hookSpeedMultiplier;
            else if (HasReachedHookTarget)
                newSpeed = 0f;
            else
                newSpeed = _currentSpeed - _windSpeed;

            return Mathf.Clamp(newSpeed, 0, MaxSpeed);
        }
    }

    private void UpdateHookLine(Vector2 endpoint) 
        => _line.SetPositions(new Vector3[] { _hookOrigin, endpoint });

    private void EnableHook() => _line.enabled = true;

    private void ClearHookLine() => _line.enabled = false;
}
