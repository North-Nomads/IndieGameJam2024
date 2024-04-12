using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    private const float GroundCheckRadius = .5f;
    private const int GroundLayer = 1 << 6;
    private const string DeathZoneTag = "Death";
    [SerializeField, Min(0)] private float moveSpeed = 1f;
    
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float fallMultiplier;

    private SpriteRenderer _spriteRenderer;
    private PlayerTentacle _tentacle;
    private Rigidbody2D _rigidbody;
    private float _horizontalInput;
    private bool _isDead;

    public event EventHandler OnPlayerDead = delegate { };

    protected Vector3 SpriteBottom => transform.position - new Vector3(0, _spriteRenderer.bounds.size.y / 2, 0);

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _tentacle = GetComponent<PlayerTentacle>();
    }

    public void FixedUpdate()
    {
        if (_isDead || _tentacle.IsHooked)
            return;

        MoveHorizontally();
    }

    private void MoveHorizontally()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _rigidbody.position += _horizontalInput * moveSpeed * Time.fixedDeltaTime * Vector2.right;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(DeathZoneTag))
        {
            _isDead = true;
            OnPlayerDead(this, null);
        }
    }
}