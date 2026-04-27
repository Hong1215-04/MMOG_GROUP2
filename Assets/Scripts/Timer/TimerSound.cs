using Unity.VisualScripting;
using UnityEngine;

public class TimerSound : MonoBehaviour
{
    [SerializeField] private AudioSource _timerSound, _endSound;
    [SerializeField] Timer timer;

    private void OnEnable()
    {
        timer.OnTimeEntersLow += PlaySound;
        timer.OnTimeExitsLow += StopSound;
    }

    void PlaySound()
    {
        if (_timerSound != null)
        {
            _timerSound.Play();
        }
    }

    void StopSound()
    {
        if (_timerSound != null)
        {
            _timerSound.Stop();
        }
    }

    void PlayEndSound()
    {
        if(_timerSound != null)
        {
            _timerSound.Stop();
        }
        if (_endSound != null) 
        {
            _endSound.Play();
        }
    }

}
