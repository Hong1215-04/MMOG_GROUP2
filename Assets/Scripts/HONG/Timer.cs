using UnityEngine;
using TMPro;

public class Timer:MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float Timeleft;

    void Update()
    {
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
