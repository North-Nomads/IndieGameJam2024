using System;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private bool _isTimerRunning = false;
    private float _startTime;

    public string TimeText => timerText.text;

    private void Start()
    {
        if (timerText == null)
            throw new ArgumentNullException("timerText is null");
    }

    private void Update()
    {
        // Update the timer if it's running
        if (_isTimerRunning)
        {
            float elapsedTime = Time.time - _startTime;
            UpdateTimerText(elapsedTime);
        }
    }

    public void StartTimer()
    {
        // Start the timer
        _startTime = Time.time;
        _isTimerRunning = true;
    }

    public void StopTimer()
    {
        // Stop the timer
        _isTimerRunning = false;
    }

    private void UpdateTimerText(float timeElapsed)
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);
        int milliseconds = Mathf.FloorToInt((timeElapsed * 1000) % 1000);

        string timeString = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
        timerText.text = timeString;
    }
}