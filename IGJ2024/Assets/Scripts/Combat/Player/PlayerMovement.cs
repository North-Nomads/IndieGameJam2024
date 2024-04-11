using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Min(0)] private float moveSpeed = 1f;
    
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float jumpStrength;

    private float _dashTimeLeft;
    private bool IsDashing => _dashTimeLeft > 0;

    private Rigidbody2D _rigidbody;
    private float _horizontalInput;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
            PerformDash();

        if (Input.GetKeyDown(KeyCode.Space))
            PerformJump();
    }

    private void PerformJump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpStrength);
    }

    private void PerformDash() => _dashTimeLeft = dashDuration;
}