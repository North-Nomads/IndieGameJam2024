using System;
using UnityEngine;

/// <summary>
/// Mob spawner class is responsible for instantiating mobs on level. Single entity per scene
/// </summary>
public class MobSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;

    public event EventHandler<Transform> OnPlayerLoaded = delegate { };

    private void Start()
    {
        OnPlayerLoaded(this, player);
    }
}