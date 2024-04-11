using UnityEngine;

public class TestZombie : MobBehaviour
{
    private const float GroundCheckRadius = .5f;
    private const int GroundLayer = 1 << 6;

    private Vector2 direction;
    private float distance;

    public bool IsGrounded => Physics2D.OverlapCircle(SpriteBottom, GroundCheckRadius, GroundLayer);

    private void FixedUpdate()
    {
        if (IsGrounded)
            MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        // Direction vector pointing from the mob to the player
        direction.x = PlayerInstance.position.x - transform.position.x;
        distance = Mathf.Abs(direction.x);
        
        if (distance < Mob.AttackRadius)
            return;

        // Move towards the player
        direction.Normalize();
        Rigidbody.MovePosition(Rigidbody.position + Mob.SeekSpeed * Time.fixedDeltaTime * direction);
    }
}