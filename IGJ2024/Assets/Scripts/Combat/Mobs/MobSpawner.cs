using System;
using UnityEngine;

/// <summary>
/// Mob spawner class is responsible for instantiating mobs on level. Single entity per scene
/// </summary>
public class MobSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;

    public event EventHandler<GameObject> OnPlayerLoaded = delegate { };

    private void Start()
    {
        OnPlayerLoaded(this, player);
    }
}