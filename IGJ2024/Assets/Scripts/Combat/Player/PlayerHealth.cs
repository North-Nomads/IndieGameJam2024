using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private const string DeathZoneTag = "Death";
    private const string EscapeTag = "Escape";

    public event EventHandler OnPlayerDead = delegate { };
    public event EventHandler OnPlayerEscaped = delegate { };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(DeathZoneTag))
            OnPlayerDead(this, null);

        else if (collision.CompareTag(EscapeTag))
            OnPlayerEscaped(this, null);
    }
}
