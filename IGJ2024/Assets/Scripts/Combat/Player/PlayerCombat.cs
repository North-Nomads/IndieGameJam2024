using UnityEngine;

public class PlayerCombat : MonoBehaviour, IHittable
{
    [SerializeField, Min(0)] private float debugDamage = 1f;
    [SerializeField, Min(0)] private float maximumHealth;

    private const int EnemyLayer = 7;

    private float _currentHealth;
    public float CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = value;
            if (_currentHealth <= 0)
                HandleDeath();
        }
    }

    private void Start()
    {
        CurrentHealth = maximumHealth;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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

    public void GetHit(float damage)
    {
        CurrentHealth -= damage;
    }   

    public void HandleDeath()
    {
        Debug.Log("Player is dead");
    }
}