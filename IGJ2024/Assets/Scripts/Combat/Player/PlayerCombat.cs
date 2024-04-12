﻿using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, IHittable
{
    [SerializeField, Min(0)] private float debugDamage = 1f;
    [SerializeField, Min(0)] private float maximumHealth;

    public event EventHandler OnPlayerDead = delegate { };

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

    public bool IsDead => CurrentHealth <= 0;

    private void Start()
    {
        CurrentHealth = maximumHealth;
    }

    private void Update()
    {
        if (IsDead)
            return;
    }

    public void GetHit(float damage)
    {
        CurrentHealth -= damage;
    }   

    public void HandleDeath()
    {
        // Play death animation
        // Show death UI
        OnPlayerDead(this, null);
    }
}