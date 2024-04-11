using System;
using UnityEngine;

/// <summary>
/// Abstract class that defines any mob behaviour
/// </summary>
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(SpriteRenderer))]
public abstract class MobBehaviour : MonoBehaviour, IHittable
{
    [SerializeField] private MobParameters mob;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private float _currentHealth;
    private float _attackCooldownLeft;

    public PlayerCombat PlayerInstance { get; internal set; }
    protected Animator Animator { get => _animator; }
    protected Rigidbody2D Rigidbody { get => _rigidbody; }
    protected MobParameters Mob { get => mob; }
    protected Vector3 SpriteBottom => transform.position - new Vector3(0, _spriteRenderer.bounds.size.y / 2, 0);
    protected float MaxHealth { get; private set; }
    protected float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            if (_currentHealth <= 0)
                HandleDeath();
        }
    }
    protected bool CanAttackNow => _attackCooldownLeft <= 0;
    protected float DistanceToPlayer => Mathf.Abs(PlayerInstance.transform.position.x - transform.position.x);

    public abstract void GetHit(float damage);
    public abstract void HandleDeath();

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        MaxHealth = Mob.HealthPoints;
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (_attackCooldownLeft > 0)
            _attackCooldownLeft -= Time.deltaTime;
    }

    protected void ResetAttackCooldown()
    {
        _attackCooldownLeft = Mob.AttackCooldown;
    }
}