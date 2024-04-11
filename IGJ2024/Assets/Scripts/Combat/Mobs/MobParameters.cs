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
    [SerializeField, Min(0)] private float attackDelay;
    [SerializeField, Min(0)] private float attackAnimationHitDelay;
    [SerializeField, Min(0)] private float attackDamage;
    [SerializeField, Min(0)] private float chaseSpeedSpeed;

    /// <summary>
    /// Mob health points
    /// </summary>
    public float HealthPoints { get => healthPoints; set => healthPoints = value; }

    /// <summary>
    /// Mob speed during roaming
    /// </summary>
    public float SeekSpeed { get => seekSpeed; set => seekSpeed = value; }

    /// <summary>
    /// Mob range within which he starts chasing player
    /// </summary>
    public float SeekRadius { get => seekRadius; set => seekRadius = value; }

    /// <summary>
    /// Mob range within which he starts performing attack
    /// </summary>
    public float AttackRadius { get => attackRadius; set => attackRadius = value; }

    /// <summary>
    /// Delay in seconds between two attacks
    /// </summary>
    public float AttackDelay { get => attackDelay; set => attackDelay = value; }

    /// <summary>
    /// A delay in seconds after which the damage will be applied
    /// </summary>
    public float AttackAnimationHitDelay { get => attackAnimationHitDelay; set => attackAnimationHitDelay = value; }
    
    /// <summary>
    /// Damage mob deals to the player
    /// </summary>
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }

    /// <summary>
    /// Mob speed during chasing player status
    /// </summary>
    public float ChaseSpeedSpeed { get => chaseSpeedSpeed; set => chaseSpeedSpeed = value; }
}