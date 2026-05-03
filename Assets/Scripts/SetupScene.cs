using UnityEngine;
using TMPro;
using System.Collections;

public class SetupScene : MonoBehaviour
{
    [SerializeField] Transform spawnPoint1, spawnPoint2;
    [SerializeField] Transform p1SkillsIconHolder, p2SkillsIconHolder;
    [SerializeField] Transform p1SkillInitPos, p1SkillFinalPos, p2SkillInitPos, p2SkillFinalPos;
    [SerializeField] float uiMovingSpeed, timerMoveSpeed;
    [SerializeField] Timer timer;
    [SerializeField] Transform timerInitPos, timerEndPos;
    [SerializeField] TMP_Text countdownText;
    [SerializeField] Transform countdownInitPos, countdownMidPos, countdownEndPos;
    [SerializeField] float countdownDuration = 1f;
    [SerializeField] float countdownHoldDelay = 0.3f;
    [SerializeField] AudioSource countdown;
    private float countdownSlideSpeed;

    void Start()
    {
        StartCoroutine(SetupSequence());
    }

    IEnumerator SetupSequence()
    {
        float slideTime = countdownDuration - countdownHoldDelay;
        countdownSlideSpeed = Vector3.Distance(countdownInitPos.position, countdownMidPos.position) / (slideTime * 0.5f);

        Coroutine c1 = StartCoroutine(UIAnimator.SlideToPosition(p1SkillsIconHolder, p1SkillInitPos.position, p1SkillFinalPos.position, uiMovingSpeed));
        Coroutine c2 = StartCoroutine(UIAnimator.SlideToPosition(p2SkillsIconHolder, p2SkillInitPos.position, p2SkillFinalPos.position, uiMovingSpeed));
        Coroutine c3 = StartCoroutine(UIAnimator.SlideToPosition(timer.transform, timerInitPos.position, timerEndPos.position, timerMoveSpeed));

        yield return c1;
        yield return c2;
        yield return c3;

        yield return StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        string[] steps = { "3", "2", "1", "FIGHT!" };

        foreach (string step in steps)
        {
            countdownText.text = step;
            countdownText.gameObject.SetActive(true);
            countdownText.transform.position = countdownInitPos.position;

            if (step == "3")
            {
                countdown.Play();
            }
            yield return StartCoroutine(UIAnimator.SlideToPosition(
                countdownText.transform,
                countdownInitPos.position,
                countdownMidPos.position,
                countdownSlideSpeed
            ));

            // Hold
            yield return new WaitForSeconds(countdownHoldDelay);

            // Slide out to end
            yield return StartCoroutine(UIAnimator.SlideToPosition(
                countdownText.transform,
                countdownMidPos.position,
                countdownEndPos.position,
                countdownSlideSpeed
            ));

            countdownText.gameObject.SetActive(false);
        }

        //timer.StartTimer();
    }
}