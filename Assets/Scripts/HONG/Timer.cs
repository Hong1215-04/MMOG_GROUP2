using UnityEngine;
using TMPro;
using System;

public class Timer:MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float Timeleft;
    [SerializeField] float lowTime;
    public Action OnTimeEntersLow, OnTimeExitsLow, OnTimeEnded;
    bool isLow=false;


    void Update()
    {
        if(Timeleft < lowTime && !isLow)
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
        //remember % is what left after divide 
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ExitGame()
    {
        //Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
