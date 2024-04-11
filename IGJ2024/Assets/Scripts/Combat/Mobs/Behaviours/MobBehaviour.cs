using UnityEngine;

/// <summary>
/// Abstract class that defines any mob behaviour
/// </summary>
[RequireComponent(typeof(Animator))]
public abstract class MobBehaviour : MonoBehaviour
{
    [SerializeField] private MobParameters mob;
    private Animator _animator;

    public GameObject PlayerInstance { get; internal set; }
    public Animator Animator { get => _animator; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
}