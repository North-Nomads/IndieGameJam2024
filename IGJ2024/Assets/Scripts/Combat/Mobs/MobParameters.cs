using UnityEngine;

[CreateAssetMenu(fileName = "MobParams", menuName = "ScriptableObjects/Mobs", order = 1)]
public class MobParameters : ScriptableObject
{
    [SerializeField, Min(0)] private float healthPoints;

    [Header("Roaming")]
    [SerializeField, Min(0)] private float seekSpeed;
    [SerializeField, Min(0)] private float seekRadius;

    [Header("Combat")]
    [SerializeField, Min(0)] private float attackRadius;
    [SerializeField, Min(0)] private float attackCooldown;
    [SerializeField, Min(0)] private float attackAnimationDelay;
    [SerializeField, Min(0)] private float attackDamage;
    [SerializeField, Min(0)] private float chaseSpeedSpeed;
    [SerializeField, Min(0)] private float hitKnockbackStrength;


    /// <summary>
    /// Mob health points
    /// </summary>
    public float HealthPoints => healthPoints;

    /// <summary>
    /// Mob speed during roaming
    /// </summary>
    public float SeekSpeed => seekSpeed;  /// <summary>
    /// Mob range within which he starts chasing player
    /// </summary>
    public float SeekRadius => seekRadius;

    /// <summary>
    /// Mob range within which he starts performing attack
    /// </summary>
    public float AttackRadius => attackRadius;

    /// <summary>
    /// Delay in seconds between two attacks
    /// </summary>
    public float AttackCooldown => attackCooldown;

    /// <summary>
    /// A delay in seconds after which the damage will be applied
    /// </summary>
    public float AttackAnimationDelay => attackAnimationDelay;
    
    /// <summary>
    /// Damage mob deals to the player
    /// </summary>
    public float AttackDamage => attackDamage;

    /// <summary>
    /// Mob speed during chasing player status
    /// </summary>
    public float ChaseSpeedSpeed => chaseSpeedSpeed;

    /// <summary>
    /// Stength will be applied on enemy gets hit
    /// </summary>
    public float HitKnockbackStrength => hitKnockbackStrength;
}