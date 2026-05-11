using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource bgmSource;

    [Header("BGM Clips")]
    public AudioClip menuBGM;
    public AudioClip characterSelectBGM;
    public AudioClip inGameBGM;

    [Header("Settings")]
    [Range(0f, 2f)]
    public float fadeDuration = 1.0f;

    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMenuBGM();
    }

    public void PlayMenuBGM()           => PlayBGM(menuBGM);
    public void PlayCharacterSelectBGM() => PlayBGM(characterSelectBGM);
    public void PlayInGameBGM()          => PlayBGM(inGameBGM);

    void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource.clip == clip) return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeAndSwitch(clip));
    }

    System.Collections.IEnumerator FadeAndSwitch(AudioClip newClip)
    {
        float startVolume = bgmSource.volume;
        float timer = 0f;

        // 淡出
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        // 切换并淡入
        bgmSource.clip = newClip;
        bgmSource.Play();
        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, startVolume, timer / fadeDuration);
            yield return null;
        }

        bgmSource.volume = startVolume;
    }
}