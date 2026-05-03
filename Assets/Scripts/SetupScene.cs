using UnityEngine;
using TMPro;
using System.Collections;

public class SetupScene : MonoBehaviour
{
    [SerializeField] CameraFollow follow;
    [SerializeField] CameraZoom zoom;
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


    void SetupIconValues(GameObject icon, PlayerAbility ability)
    {
        PlayerAbilityFill fill = icon.GetComponent<PlayerAbilityFill>();
        PlayerAbilityIconEffects effects = icon.GetComponent<PlayerAbilityIconEffects>();

        if (effects != null)
        {
            effects.ability = ability;
        }
        if (fill != null)
        {
            fill.ability = ability;
        }
    }

    private void Awake()
    {
        CharacterSlot p1 = MatchData.Player1Character;
        CharacterSlot p2 = MatchData.Player2Character;

        GameObject p1Player = Instantiate(p1.playerPrefab, spawnPoint1.position, spawnPoint1.rotation);
        GameObject p2Player = Instantiate(p2.playerPrefab, spawnPoint2.position, spawnPoint2.rotation);

        CharacterAbilityInfo p1Info = p1Player.GetComponent<CharacterAbilityInfo>();
        CharacterAbilityInfo p2Info = p2Player.GetComponent<CharacterAbilityInfo>();

        PlayerMovement p1Movement = p1Player.GetComponent<PlayerMovement>();
        PlayerMovement p2Movement = p2Player.GetComponent<PlayerMovement>();
        p1Movement.keyLeft = KeyCode.A;
        p1Movement.keyRight = KeyCode.D;
        p2Movement.keyLeft = KeyCode.LeftArrow;
        p2Movement.keyRight= KeyCode.RightArrow;

        PlayerJump p1Jump = p1Player.GetComponent<PlayerJump>();
        PlayerJump p2Jump = p2Player.GetComponent <PlayerJump>();
        p1Jump.keyJump = KeyCode.W;
        p2Jump.keyJump = KeyCode.UpArrow;


        follow.Player1 = p1Player.transform;
        follow.Player2 = p2Player.transform;
        zoom.Player1 = p1Player;
        zoom.Player2 = p2Player;

        foreach (CharacterAbility ability in p1Info.abilities)
        {
            if (ability.icon == null) continue;
            GameObject icon = Instantiate(ability.icon, p1SkillsIconHolder);
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localRotation = Quaternion.identity;
            icon.transform.localScale = Vector3.one;
            SetupIconValues(icon, ability.ability);
        }

        foreach (CharacterAbility ability in p2Info.abilities)
        {
            if (ability.icon == null) continue;
            GameObject icon = Instantiate(ability.icon, p2SkillsIconHolder);
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localRotation = Quaternion.identity;
            icon.transform.localScale = Vector3.one;
            SetupIconValues(icon, ability.ability);
        }
    }

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