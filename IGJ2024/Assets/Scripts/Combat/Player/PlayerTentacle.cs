using System.Collections;
using UnityEngine;

public class PlayerTentacle : MonoBehaviour
{
    [SerializeField] private float hookRange;
    [SerializeField] private LayerMask hookSurface;
    [SerializeField] private LayerMask possibleHookTargets;
    [SerializeField] private float hookSpeedMultiplier;
    [SerializeField] private float hookGrowthSpeed;
    [SerializeField] private AnimationCurve hookSpeed;
    [SerializeField] private float hookBoostTime;

    private float _hookMountElapsedTime;
    private LineRenderer _line;
    private Rigidbody2D _rigidbody;
    private bool _isHooking;
    private Vector2 _hookTarget;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        _line = GetComponent<LineRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(_rigidbody.position, mousePosition - _rigidbody.position, hookRange, possibleHookTargets.value);

            bool hasMissed = hit.collider == null;
            bool isHitSuccessful = false;
            
            // By default, player has missed.
            // The tentacle will try to reach the point in that direction by the tentacle range
            _hookTarget = mousePosition;

            // In case if player has hit something
            if (!hasMissed)
            {
                // In case if the player hit the pipes - he successfully sucks on it
                isHitSuccessful = Physics2D.OverlapCircle(hit.point, .1f, hookSurface.value);
                _hookTarget = hit.point; // the point will be overwritten in any case
            }

            EnableHook();
            StartCoroutine(LaunchTentacle(isHitSuccessful));
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isHooking = false;
            ClearHookLine();
            _hookMountElapsedTime = 0f;
        }
    }

    private IEnumerator LaunchTentacle(bool hasHit)
    {
        var direction = (_rigidbody.position - _hookTarget).normalized;

        var currentPoint = _rigidbody.position;
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
    }

    private void FixedUpdate()
    {
        if (_isHooking)
            RideHook();
    }

    private void RideHook()
    {
        float currentSpeed = EvaluateSpeed();

        Vector2 hookDirection = (_hookTarget - (Vector2)transform.position).normalized;
        _rigidbody.velocity = hookDirection * currentSpeed;
        UpdateHookLine(_hookTarget);

        float EvaluateSpeed()
        {
            float clamp = Mathf.Clamp01(_hookMountElapsedTime / hookBoostTime);
            float currentSpeed = hookSpeed.Evaluate(clamp) * hookSpeedMultiplier;
            _hookMountElapsedTime += Time.fixedDeltaTime;
            return currentSpeed;
        }
    }

    private void UpdateHookLine(Vector2 endpoint) 
        => _line.SetPositions(new Vector3[] { transform.position + Vector3.up * .5f, endpoint });

    private void EnableHook() => _line.enabled = true;

    private void ClearHookLine() => _line.enabled = false;
}
