using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Min(0)] private float moveSpeed = 1f;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        _rigidbody.MovePosition(_rigidbody.position + horizontal * moveSpeed * Time.fixedDeltaTime * Vector2.right);
    }
}
