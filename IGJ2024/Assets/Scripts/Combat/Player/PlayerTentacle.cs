using UnityEngine;

public class PlayerTentacle : MonoBehaviour
{
    [SerializeField] private float hookRange;
    [SerializeField] private LayerMask hookSurface;
    [SerializeField] private float hookSpeed;
    private Rigidbody2D _rigidbody;
    private bool _isHooking;
    private Vector2 _hookTarget;

    public bool IsHooked => _isHooking;

    private void Start() => _rigidbody = GetComponent<Rigidbody2D>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePosition - _rigidbody.position, hookRange, hookSurface.value);
            if (hit.collider != null)
            {
                _isHooking = true;
                _hookTarget = hit.point;
            }
        }

        if (Input.GetMouseButtonUp(0))
            _isHooking = false;
    }

    private void FixedUpdate()
    {
        if (_isHooking)
        {
            Vector2 hookDirection = (_hookTarget - (Vector2)transform.position).normalized;
            _rigidbody.velocity = hookDirection * hookSpeed;
        }
    }
}
