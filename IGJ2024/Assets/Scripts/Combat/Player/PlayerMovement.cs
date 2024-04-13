using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    private const string DeathZoneTag = "Death";
    private const string EscapeTag = "Escape";

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

    public event EventHandler OnPlayerDead = delegate { };
    public event EventHandler OnPlayerEscaped = delegate { };

    protected Vector3 SpriteBottom => transform.position - new Vector3(0, _spriteRenderer.bounds.size.y / 2, 0);
    private bool IsGrounded => Physics2D.OverlapBox(SpriteBottom, feetBox, 0, groundLayer);

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerVFX = GetComponent<PlayerVFX>();
    }

    public void FixedUpdate()
    {
        _animator.SetBool("IsFloating", !IsGrounded);
        
        if (!_endedGrounded && IsGrounded)
            _playerVFX.SpawnLandingVFX(SpriteBottom);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(DeathZoneTag))
            OnPlayerDead(this, null);

        else if (collision.CompareTag(EscapeTag))
            OnPlayerEscaped(this, null);
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