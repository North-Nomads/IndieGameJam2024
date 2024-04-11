using UnityEngine;

/// <summary>
/// Abstract class that defines any mob behaviour
/// </summary>
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(SpriteRenderer))]
public abstract class MobBehaviour : MonoBehaviour
{
    [SerializeField] private MobParameters mob;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    public Transform PlayerInstance { get; internal set; }
    protected Animator Animator { get => _animator; }
    protected Rigidbody2D Rigidbody { get => _rigidbody; }
    protected MobParameters Mob { get => mob; }
    protected Vector3 SpriteBottom => transform.position - new Vector3(0, _spriteRenderer.bounds.size.y / 2, 0);

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();    
    }
}