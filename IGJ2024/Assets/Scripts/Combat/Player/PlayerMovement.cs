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

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private PlayerVFX _playerVFX;
    private Animator _animator;
    private bool _isFacingRight = true;
    private float _horizontalInput;
    private bool _endedGrounded;

    private bool _levelPaused;

    public event EventHandler OnPlayerLanded = delegate { };


    protected Vector3 SpriteBottom => transform.position - new Vector3(0, _spriteRenderer.bounds.size.y / 2, 0);
    private bool IsGrounded => Physics2D.OverlapBox(SpriteBottom, feetBox, 0, groundLayer);

    private void Start()
    {
        _levelPaused = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerVFX = GetComponent<PlayerVFX>();
        GetComponent<PlayerHealth>().OnPlayerDead += HandleLevelPaused;
        GetComponent<PlayerHealth>().OnPlayerEscaped += HandleLevelPaused;
    }

    private void HandleLevelPaused(object sender, EventArgs e)
    {
        _levelPaused = true;
    }

    public void FixedUpdate()
    {
        _animator.SetBool("IsFloating", !IsGrounded);
        
        if (!_endedGrounded && IsGrounded)
        {
            OnPlayerLanded(this, null);
            _playerVFX.SpawnLandingVFX(SpriteBottom);
        }

        _endedGrounded = IsGrounded;

        if (!IsGrounded || _levelPaused)
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