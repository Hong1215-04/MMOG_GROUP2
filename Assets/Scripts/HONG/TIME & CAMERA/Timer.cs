using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float Timeleft;
    [SerializeField] float lowTime;
    [SerializeField] GameObject P1DefenderWin;
    [SerializeField] GameObject P2AttackerWin;
    [SerializeField] Health P1Health;
    [SerializeField] Health P2Health;

    private float P1HP;
    private float P2HP;
    public Action OnTimeEntersLow, OnTimeExitsLow, OnTimeEnded;
    bool isLow = false;

    void Awake()
    {
        P1DefenderWin.SetActive(false);
        P2AttackerWin.SetActive(false);
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
            if (P2HP > P1HP)
            {
                Invoke("P2Win", 3);
            }
            else
            {
                Invoke("P1Win", 3);
            }
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

    public void P1Win()
    {
        P1DefenderWin.SetActive(true);
    }

    public void P2Win()
    {
        P2AttackerWin.SetActive(true);
    }
}