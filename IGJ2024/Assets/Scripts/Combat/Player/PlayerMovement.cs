using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Min(0)] private float moveSpeed = 1f;
    [SerializeField, Min(0)] private float debugDamage = 1f;
    private Rigidbody2D _rigidbody;
    private const int EnemyLayer = 7;

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
        else if (Input.GetKey(KeyCode.Space))
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        var collisions = Physics2D.OverlapBoxAll(transform.position, Vector2.one * 5, 0, 1 << EnemyLayer);
        foreach (var collision in collisions)
        {
            collision.GetComponent<MobBehaviour>().GetHit(debugDamage);
        }
    }
}
