using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField, Min(0)] private float debugDamage = 1f;
    private const int EnemyLayer = 7;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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