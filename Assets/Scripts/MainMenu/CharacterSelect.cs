using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerSlot
{
    public Image bigImage;
    public Transform previewPlayerPos, bigImageInitPos, bigImageFinalPos;
    public GameObject hover;
    public GameObject initialPreview;
    [HideInInspector] public GameObject previewInstance;
}

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] CharacterSlot[] characterSlots;
    [SerializeField] PlayerSlot player1slot, player2slot;
    [SerializeField] AudioSource switchAudio, confirmAudio;

    private int p1Index = 0;
    private int p2Index = 0;
    private bool p1Confirmed = false;
    private bool p2Confirmed = false;

    private Coroutine p1SlideCoroutine;
    private Coroutine p2SlideCoroutine;

    void Start()
    {
        SetHoverVisible(player1slot, true);
        SetHoverVisible(player2slot, false);

        MoveHover(player1slot, characterSlots[p1Index]);
        UpdateBigImage(player1slot, characterSlots[p1Index], ref p1SlideCoroutine);
        SpawnPreview(player1slot, characterSlots[p1Index]);
    }

    void Update()
    {
        if (!p1Confirmed)
            HandleP1Input();
        else if (!p2Confirmed)
            HandleP2Input();
    }

    void HandleP1Input()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            int next = (p1Index + 1) % characterSlots.Length;
            if (!IsOccupiedByOther(next, p2Index))
            {
                switchAudio.Play();
                p1Index = next;
                MoveHover(player1slot, characterSlots[p1Index]);
                UpdateBigImage(player1slot, characterSlots[p1Index], ref p1SlideCoroutine);
                SpawnPreview(player1slot, characterSlots[p1Index]);
            }
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            int next = (p1Index - 1 + characterSlots.Length) % characterSlots.Length;
            if (!IsOccupiedByOther(next, p2Index))
            {
                switchAudio.Play();
                p1Index = next;
                MoveHover(player1slot, characterSlots[p1Index]);
                UpdateBigImage(player1slot, characterSlots[p1Index], ref p1SlideCoroutine);
                SpawnPreview(player1slot, characterSlots[p1Index]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            ConfirmP1();
    }

    void HandleP2Input()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            int next = (p2Index + 1) % characterSlots.Length;
            if (!IsOccupiedByOther(next, p1Index))
            {
                switchAudio.Play();
                p2Index = next;
                MoveHover(player2slot, characterSlots[p2Index]);
                UpdateBigImage(player2slot, characterSlots[p2Index], ref p2SlideCoroutine);
                SpawnPreview(player2slot, characterSlots[p2Index],true);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            int next = (p2Index - 1 + characterSlots.Length) % characterSlots.Length;
            if (!IsOccupiedByOther(next, p1Index))
            {
                switchAudio.Play();
                p2Index = next;
                MoveHover(player2slot, characterSlots[p2Index]);
                UpdateBigImage(player2slot, characterSlots[p2Index], ref p2SlideCoroutine);
                SpawnPreview(player2slot, characterSlots[p2Index],true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            ConfirmP2();
    }

    bool IsOccupiedByOther(int indexToCheck, int otherIndex)
    {
        return p1Confirmed && indexToCheck == otherIndex;
    }

    void ConfirmP1()
    {
        confirmAudio.Play();
        p1Confirmed = true;

        p2Index = p2Index == p1Index
            ? (p1Index + 1) % characterSlots.Length
            : p2Index;
        player1slot.hover.GetComponent<Animator>().SetBool("Done", true);
        SetHoverVisible(player2slot, true);
        MoveHover(player2slot, characterSlots[p2Index]);
        UpdateBigImage(player2slot, characterSlots[p2Index], ref p2SlideCoroutine);
        SpawnPreview(player2slot, characterSlots[p2Index], true);

        Debug.Log($"P1 confirmed: {characterSlots[p1Index].name}");
    }

    void ConfirmP2()
    {
        confirmAudio.Play();
        p2Confirmed = true;
        player2slot.hover.GetComponent<Animator>().SetBool("Done", true);
        MatchData.SetSelections(characterSlots[p1Index], characterSlots[p2Index]);
        MatchData.p1Name = characterSlots[p1Index].name;
        Debug.Log($"P2 confirmed: {characterSlots[p2Index].name}");
        StartMatch();
    }
    void SpawnPreview(PlayerSlot playerSlot, CharacterSlot characterSlot, bool isPlayer2 = false)
    {
        if (playerSlot.initialPreview != null)
        {
            Destroy(playerSlot.initialPreview);
            playerSlot.initialPreview = null;
        }

        if (playerSlot.previewInstance != null)
            Destroy(playerSlot.previewInstance);

        if (characterSlot.previewCharacter == null || playerSlot.previewPlayerPos == null) return;

        playerSlot.previewInstance = Instantiate(
            characterSlot.previewCharacter,
            playerSlot.previewPlayerPos
        );

        playerSlot.previewInstance.transform.localPosition = Vector3.zero;
        playerSlot.previewInstance.transform.localRotation = Quaternion.identity;
        playerSlot.previewInstance.transform.localScale = Vector3.one;

        if (isPlayer2)
        {
            Debug.Log("A");
            Vector3 scale = playerSlot.previewInstance.transform.localScale;
            scale.x *= -1;

            PlaceHold tmp = playerSlot.previewInstance.GetComponentInChildren<PlaceHold>();
            if (tmp != null)
            {
                Vector3 tmpScale = tmp.transform.localScale;
                tmpScale.x *= -1;
                tmp.transform.localScale = tmpScale;
            }
        }
    }

    void UpdateBigImage(PlayerSlot playerSlot, CharacterSlot characterSlot, ref Coroutine slideCoroutine, float speed = 20f)
    {
        if (playerSlot.bigImage == null || characterSlot.bigImage == null) return;

        playerSlot.bigImage.sprite = characterSlot.bigImage;

        if (slideCoroutine != null) StopCoroutine(slideCoroutine);
        slideCoroutine = StartCoroutine(UIAnimator.SlideToPosition(
            playerSlot.bigImage.transform,
            playerSlot.bigImageInitPos.position,
            playerSlot.bigImageFinalPos.position,
            speed
        ));
    }

    void MoveHover(PlayerSlot playerSlot, CharacterSlot targetSlot)
    {
        if (playerSlot.hover == null || targetSlot == null) return;
        playerSlot.hover.transform.position = targetSlot.transform.position;
    }

    void SetHoverVisible(PlayerSlot playerSlot, bool visible)
    {
        if (playerSlot.hover != null)
            playerSlot.hover.SetActive(visible);
    }

    void StartMatch()
    {
        Debug.Log("Both players confirmed. Starting match!");
        StartCoroutine(loadScene());
    }

    IEnumerator loadScene()
    {
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapCamera");
    }
}