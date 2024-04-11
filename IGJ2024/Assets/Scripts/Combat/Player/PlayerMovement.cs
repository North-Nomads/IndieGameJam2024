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

    public void Update()
    {
        // Нормально переписать, debug only
        if (Input.GetKey(KeyCode.A))
        {
            _rigidbody.MovePosition(_rigidbody.position + moveSpeed * Time.fixedDeltaTime * Vector2.left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _rigidbody.MovePosition(_rigidbody.position + moveSpeed * Time.fixedDeltaTime * Vector2.right);
        }
    }
}
