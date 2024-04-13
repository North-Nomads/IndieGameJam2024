using System;
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
    private float _horizontalInput;
    private bool _canMove;
    private bool _isFacingRight = true;
    private Animator _animator;

    public event EventHandler OnPlayerDead = delegate { };
    public event EventHandler OnPlayerEscaped = delegate { };

    protected Vector3 SpriteBottom => transform.position - new Vector3(0, _spriteRenderer.bounds.size.y / 2, 0);
    private bool IsGrounded => !Physics2D.OverlapBox(SpriteBottom, feetBox, 0, groundLayer);

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        if (_canMove || IsGrounded)
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
        {
            _canMove = true;
            OnPlayerDead(this, null);
        }

        else if (collision.CompareTag(EscapeTag))
        {
            OnPlayerEscaped(this, null);
        }
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
}