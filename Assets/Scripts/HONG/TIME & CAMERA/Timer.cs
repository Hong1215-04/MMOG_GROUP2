using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float Timeleft;
    [SerializeField] float lowTime;
    public Action OnTimeEntersLow, OnTimeExitsLow, OnTimeEnded;
    bool isLow = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (Timeleft < lowTime && !isLow)
        {
            isLow = true;
            OnTimeEntersLow?.Invoke();
        }
        if (Timeleft > lowTime && isLow)
        {
            isLow = false;
            OnTimeExitsLow?.Invoke();
        }
        if (Timeleft > 0)
        {
            Timeleft -= Time.deltaTime;
        }
        else if (Timeleft == 0)
        {
            Invoke("ExitGame", 2);
        }
        else if (Timeleft < 0)
        {
            Timeleft = 0;
            timerText.color = Color.red;
            OnTimeEnded?.Invoke();
        }
        int minutes = Mathf.FloorToInt(Timeleft / 60);
        int seconds = Mathf.FloorToInt(Timeleft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddTime(float amount)
    {
        Timeleft += amount;
        if (Timeleft > 0 && isLow)
        {
            isLow = false;
            OnTimeExitsLow?.Invoke();
        }
    }
    public void SubtractTime(float amount)
    {
        Timeleft = Mathf.Max(0, Timeleft - amount);
        if (Timeleft < lowTime && !isLow)
        {
            isLow = true;
            OnTimeEntersLow?.Invoke();
        }
    }

    public void ExitGame()
    {
        //Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}