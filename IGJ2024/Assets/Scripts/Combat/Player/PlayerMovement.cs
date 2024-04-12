using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerCombat), typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    private const float GroundCheckRadius = .5f;
    private const int GroundLayer = 1 << 6;

    [SerializeField, Min(0)] private float moveSpeed = 1f;
    
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float jumpStrength;

    private float _dashTimeLeft;
    private bool IsDashing => _dashTimeLeft > 0;

    private Rigidbody2D _rigidbody;
    private PlayerCombat _playerCombat;
    private float _horizontalInput;
    private SpriteRenderer _spriteRenderer;

    protected Vector3 SpriteBottom => transform.position - new Vector3(0, _spriteRenderer.bounds.size.y / 2, 0);
    private bool IsGrounded => Physics2D.OverlapCircle(SpriteBottom, GroundCheckRadius, GroundLayer);

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerCombat = GetComponent<PlayerCombat>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FixedUpdate()
    {
        if (_playerCombat.IsDead)
            return;

        if (IsDashing)
        {
            _rigidbody.position += _horizontalInput * dashSpeed * Time.fixedDeltaTime * Vector2.right;
            _dashTimeLeft -= Time.fixedDeltaTime;
        }
        else
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _rigidbody.position += _horizontalInput * moveSpeed * Time.fixedDeltaTime * Vector2.right;
        }
    }

    private void Update()
    {
        if (_playerCombat.IsDead)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            PerformDash();

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            PerformJump();
    }

    private void PerformJump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpStrength);
    }

    private void PerformDash() => _dashTimeLeft = dashDuration;
}