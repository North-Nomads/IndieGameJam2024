using UnityEngine;

public class TestZombie : MobBehaviour 
{
    private const float GroundCheckRadius = .5f;
    private const int GroundLayer = 1 << 6;

    private Vector2 _directionTowardsPlayer;

    public bool IsGrounded => Physics2D.OverlapCircle(SpriteBottom, GroundCheckRadius, GroundLayer);
    
    private void FixedUpdate()
    {
        if (IsGrounded)
            MoveTowardsPlayer();

        if (DistanceToPlayer <= Mob.AttackRadius)
            PerformAttack();
    }

    private void PerformAttack()
    {
        if (!CanAttackNow)
            return;
        
        // Play animation
        // Wait for the delay
        // Apply damage after mob.attackdelay
        PlayerInstance.GetHit(Mob.AttackDamage);
        ResetAttackCooldown();
    }

    public override void GetHit(float damage)
    {
        CurrentHealth -= damage;
        Rigidbody.MovePosition(Rigidbody.position + Mob.HitKnockbackStrength * Time.fixedDeltaTime * -_directionTowardsPlayer);
    }

    public override void HandleDeath() => Destroy(gameObject);

    private void MoveTowardsPlayer()
    {
        // Direction vector pointing from the mob to the player
        _directionTowardsPlayer.x = PlayerInstance.transform.position.x - transform.position.x;
        
        if (DistanceToPlayer < Mob.AttackRadius)
            return;

        // Move towards the player
        _directionTowardsPlayer.Normalize();
        Rigidbody.MovePosition(Rigidbody.position + Mob.SeekSpeed * Time.fixedDeltaTime * _directionTowardsPlayer);
    }
}