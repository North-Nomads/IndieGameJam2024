using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float fadeDuration;

    private const string DeathZoneTag = "Death";
    private const string EscapeTag = "Escape";
    private SpriteRenderer _playerRenderer;

    public event EventHandler OnPlayerDead = delegate { };
    public event EventHandler OnPlayerEscaped = delegate { };

    private void Start()
    {
        _playerRenderer = GetComponent<SpriteRenderer>();
    }

    internal void PlayFadeAnimation()
    {
        StartCoroutine(DisappearAnimation());

        IEnumerator DisappearAnimation()
        {
            var timeElapsed = 0f;
            Color startColor = _playerRenderer.color;
            Color endColor = new(startColor.r, startColor.g, startColor.b, 0f);
            while (timeElapsed < fadeDuration)
            {
                _playerRenderer.color = Color.Lerp(startColor, endColor, timeElapsed /  fadeDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            _playerRenderer.color = endColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(DeathZoneTag))
            OnPlayerDead(this, null);

        else if (collision.CompareTag(EscapeTag))
            OnPlayerEscaped(this, null);
    }
}
