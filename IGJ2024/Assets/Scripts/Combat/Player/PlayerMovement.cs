using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(PlayerHealth))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Min(0)] private float moveSpeed = 1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private Vector2 feetBox;
    [SerializeField] private Transform feetPosition;

    private Rigidbody2D _rigidbody;
    private PlayerVFX _playerVFX;
    private Animator _animator;
    private bool _isFacingRight = true;
    private float _horizontalInput;
    private bool _endedGrounded;

    public event EventHandler OnPlayerLanded = delegate { };

    private bool IsGrounded => Physics2D.OverlapBox(feetPosition.position, feetBox, 0, groundLayer);

    private void Start()
    {
        print("Start");
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerVFX = GetComponent<PlayerVFX>();

        LevelObserver.OnLevelPaused += HandleLevelPause;
    }

    private void HandleLevelPause(object sender, EventArgs e)
    {
        _animator.SetBool("IsFloating", false);
        _animator.SetBool("IsMoving", false);
        _horizontalInput = 0f;
        _rigidbody.velocity = Vector2.zero;
    }

    public void FixedUpdate()
    {
        if (LevelObserver.IsLevelPaused)
            return;

        _animator.SetBool("IsFloating", !IsGrounded);
        
        if (!_endedGrounded && IsGrounded)
        {
            OnPlayerLanded(this, null);
            _playerVFX.SpawnLandingVFX(feetPosition.position);
        }

        _endedGrounded = IsGrounded;

        if (!IsGrounded)
            return;

        MoveHorizontally();
    }

    

    private void MoveHorizontally()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _rigidbody.velocity = new Vector2(_horizontalInput * moveSpeed, _rigidbody.velocity.y);
        _animator.SetBool("IsMoving", true);
        if (_horizontalInput != 0)
            Flip();
        else
            _animator.SetBool("IsMoving", false);
    }

    private void Flip()
    {
        if (_isFacingRight && _horizontalInput < 0f || !_isFacingRight && _horizontalInput > 0f)
        {
            var yValue = _isFacingRight ? 180f : 0f;
            _isFacingRight = !_isFacingRight;
            Vector3 rotator = new(transform.rotation.x, yValue, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
    }

    public void TurnTowards(bool towardsLeft)
    {
        _isFacingRight = !_isFacingRight;
        if (towardsLeft)
        {
            Vector3 rotator = new(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
        else
        {
            Vector3 rotator = new(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
    }
}