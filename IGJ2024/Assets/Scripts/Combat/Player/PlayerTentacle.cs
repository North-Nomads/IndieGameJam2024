using UnityEngine;

public class PlayerTentacle : MonoBehaviour
{
    [SerializeField] private float hookRange;
    [SerializeField] private LayerMask hookSurface;
    [SerializeField] private float hookSpeed;
    private LineRenderer _line;
    private Rigidbody2D _rigidbody;
    private bool _isHooking;
    private Vector2 _hookTarget;

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

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

            DrawHookLine();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isHooking = false;
            ClearHookLine();
        }
    }

    private void FixedUpdate()
    {
        if (_isHooking)
        {
            Vector2 hookDirection = (_hookTarget - (Vector2)transform.position).normalized;
            _rigidbody.velocity = hookDirection * hookSpeed;
            DrawHookLine();
        }
    }

    private void DrawHookLine()
    {
        _line.enabled = true;
        _line.SetPositions(new Vector3[] { transform.position + Vector3.up * .5f, _hookTarget });
    }

    private void ClearHookLine()
    {
        _line.enabled = false;
    }
}
