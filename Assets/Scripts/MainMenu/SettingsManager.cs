using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    // UI References
    [Header("Audio Settings")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Graphics Settings")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;

    [Header("Controls Settings")]
    public Button controlsButton;

    [Header("Miscellaneous")]
    public Button resetButton;
    public Button applyButton;
    public Button backButton;

    // Panel References
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    // Audio Mixer (assign in Inspector if using)
    public AudioMixer audioMixer;

    // Main menu script reference
    private MainMenu mainMenuScript;

    // Resolution options
    private List<Resolution> resolutions = new List<Resolution>();

    void Start()
    {
        InitializeAudioDefaults();
        AutoFindComponents();
        PopulateResolutionDropdown();
        LoadSettings();

        if (resetButton != null)
            resetButton.onClick.AddListener(ResetToDefaults);
        if (applyButton != null)
            applyButton.onClick.AddListener(ApplySettings);
        if (backButton != null)
            backButton.onClick.AddListener(Back);

        if (resetButton != null)
        resetButton.onClick.AddListener(() => SFXManager.Instance?.PlayButtonClick());
        if (applyButton != null)
            applyButton.onClick.AddListener(() => SFXManager.Instance?.PlayButtonClick());
        if (backButton != null)
            backButton.onClick.AddListener(() => SFXManager.Instance?.PlayButtonClick());

        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(delegate { ApplyAudioSettings(); });
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(delegate { ApplyAudioSettings(); });
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(delegate { ApplyAudioSettings(); });

        if (resolutionDropdown != null)
            resolutionDropdown.onValueChanged.AddListener(delegate { ApplyGraphicsSettings(); });
    }

    void InitializeAudioDefaults()
    {
        // If audio is at default 1.0 from before, reset to 0.6
        float masterVol = PlayerPrefs.GetFloat("MasterVolume", 0.6f);
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 0.6f);

        // If all are 1.0, it's likely old data, reset to 0.6
        if (masterVol == 1.0f && musicVol == 1.0f && sfxVol == 1.0f)
        {
            PlayerPrefs.SetFloat("MasterVolume", 0.6f);
            PlayerPrefs.SetFloat("MusicVolume", 0.6f);
            PlayerPrefs.SetFloat("SFXVolume", 0.6f);
            PlayerPrefs.Save();
            Debug.Log("Audio defaults reset to 0.6");
        }
    }

    void AutoFindComponents()
    {
        // Find MainMenu script reference
        if (mainMenuScript == null)
        {
            mainMenuScript = FindObjectOfType<MainMenu>();
            if (mainMenuScript != null)
                Debug.Log("Found MainMenu script");
        }

        // Find Graphics Settings components - using global FindObjectOfType if not found locally
        if (resolutionDropdown == null)
        {
            // First try local search for TMP_Dropdown
            resolutionDropdown = GetComponentInChildren<TMP_Dropdown>();
            
            // If not found locally, search globally
            if (resolutionDropdown == null)
            {
                resolutionDropdown = FindObjectOfType<TMP_Dropdown>();
            }
            
            if (resolutionDropdown != null)
                Debug.Log("Auto-found resolutionDropdown: " + resolutionDropdown.gameObject.name);
            else
                Debug.LogError("Resolution Dropdown (TMP_Dropdown) not found! Create a TextMesh Pro Dropdown in your SettingPanel.");
        }
        
        if (fullscreenToggle == null)
        {
            Toggle[] toggles = GetComponentsInChildren<Toggle>();
            if (toggles.Length == 0)
                toggles = FindObjectsOfType<Toggle>();
            
            if (toggles.Length > 0)
            {
                fullscreenToggle = toggles[0];
                Debug.Log("Auto-found fullscreenToggle");
            }
        }
        
        if (vsyncToggle == null)
        {
            Toggle[] toggles = GetComponentsInChildren<Toggle>();
            if (toggles.Length == 0)
                toggles = FindObjectsOfType<Toggle>();
            
            if (toggles.Length > 1)
            {
                vsyncToggle = toggles[1];
                Debug.Log("Auto-found vsyncToggle");
            }
        }

        // Find Button components
        Button[] buttons = GetComponentsInChildren<Button>();
        if (buttons.Length == 0)
            buttons = FindObjectsOfType<Button>();
        
        if (resetButton == null && buttons.Length > 0)
            resetButton = buttons[0];
        if (applyButton == null && buttons.Length > 1)
            applyButton = buttons[1];
        if (backButton == null && buttons.Length > 2)
            backButton = buttons[2];

        // Find Slider components
        Slider[] sliders = GetComponentsInChildren<Slider>();
        if (sliders.Length == 0)
            sliders = FindObjectsOfType<Slider>();
        
        if (masterVolumeSlider == null && sliders.Length > 0)
            masterVolumeSlider = sliders[0];
        if (musicVolumeSlider == null && sliders.Length > 1)
            musicVolumeSlider = sliders[1];
        if (sfxVolumeSlider == null && sliders.Length > 2)
            sfxVolumeSlider = sliders[2];

        Debug.Log("AutoFindComponents complete. Found: " + 
            (resolutionDropdown != null ? "TMP_Dropdown " : "NO TMP_Dropdown! ") +
            (masterVolumeSlider != null ? "Sliders " : "") +
            (resetButton != null ? "Buttons " : ""));
    }

    void PopulateResolutionDropdown()
    {
        if (resolutionDropdown == null) return;

        resolutions.Clear();
        resolutionDropdown.ClearOptions();

        Resolution[] availableResolutions = Screen.resolutions;
        List<string> options = new List<string>();
        HashSet<string> seen = new HashSet<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            Resolution res = availableResolutions[i];
            string key = res.width + "x" + res.height;

            if (seen.Contains(key)) continue; 
            seen.Add(key);

            string option = res.width + " x " + res.height;
            options.Add(option);
            resolutions.Add(res);

            if (res.width == 1920 && res.height == 1080)
                currentResolutionIndex = resolutions.Count - 1;
            else if (res.width == Screen.currentResolution.width && 
                    res.height == Screen.currentResolution.height && 
                    currentResolutionIndex == 0)
                currentResolutionIndex = resolutions.Count - 1;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
    }

    void LoadSettings()
    {
        // Audio - default to 0.6 to give users room to adjust
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.6f);
        if (musicVolumeSlider != null)
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.6f);

        // Graphics
        if (resolutionDropdown != null)
        {
            // Default to 1920x1080 if no saved resolution
            int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", -1);
            
            // Find 1920x1080 if no saved resolution found
            if (resolutionIndex == -1)
            {
                for (int i = 0; i < resolutions.Count; i++)
                {
                    if (resolutions[i].width == 1920 && resolutions[i].height == 1080)
                    {
                        resolutionIndex = i;
                        break;
                    }
                }
                if (resolutionIndex == -1)
                    resolutionIndex = 0; // Fallback to first resolution
            }
            
            if (resolutionIndex < resolutions.Count)
            {
                resolutionDropdown.value = resolutionIndex;
            }
        }
        
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        if (vsyncToggle != null)
            vsyncToggle.isOn = PlayerPrefs.GetInt("VSync", 1) == 1;

        // Apply loaded settings
        ApplyAudioSettings();
        ApplyGraphicsSettings();
    }

    void ApplySettings()
    {
        // Apply and save all settings
        ApplyAudioSettings();
        ApplyGraphicsSettings();
        SaveSettings();
        Debug.Log("Settings applied and saved.");
    }

    void ApplyAudioSettings()
    {
        if (audioMixer != null)
        {
            float masterVol = masterVolumeSlider != null ? masterVolumeSlider.value : 1f;
            float musicVol  = musicVolumeSlider  != null ? musicVolumeSlider.value  : 1f;
            float sfxVol    = sfxVolumeSlider    != null ? sfxVolumeSlider.value    : 1f;

            audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(masterVol, 0.0001f)) * 20);
            audioMixer.SetFloat("MusicVolume",  Mathf.Log10(Mathf.Max(musicVol,  0.0001f)) * 20);
            audioMixer.SetFloat("SFXVolume",    Mathf.Log10(Mathf.Max(sfxVol,    0.0001f)) * 20);
        }
        else
        {

            if (masterVolumeSlider != null)
                AudioListener.volume = masterVolumeSlider.value;
            Debug.LogWarning("AudioMixer not assigned! Falling back to AudioListener volume control, which will affect all audio globally.");
        }
    }

    void ApplyGraphicsSettings()
    {
        // Set resolution
        if (resolutionDropdown != null)
        {
            int resolutionIndex = resolutionDropdown.value;
            if (resolutionIndex < resolutions.Count)
            {
                Resolution res = resolutions[resolutionIndex];
                bool isFullscreen = fullscreenToggle != null ? fullscreenToggle.isOn : true;
                Screen.SetResolution(res.width, res.height, isFullscreen);
            }
        }

        // Set VSync
        if (vsyncToggle != null)
            QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;
    }

    void SaveSettings()
    {
        // Save to PlayerPrefs
        if (masterVolumeSlider != null)
            PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        if (musicVolumeSlider != null)
            PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        if (sfxVolumeSlider != null)
            PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        if (resolutionDropdown != null)
            PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        if (fullscreenToggle != null)
            PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        if (vsyncToggle != null)
            PlayerPrefs.SetInt("VSync", vsyncToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    void ResetToDefaults()
    {
        // Reset sliders and toggles to defaults - audio at 0.6, resolution 1920x1080
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = 0.6f;
        if (musicVolumeSlider != null)
            musicVolumeSlider.value = 0.6f;
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = 0.6f;
        
        // Find and set 1920x1080 resolution
        if (resolutionDropdown != null)
        {
            int resolution1080Index = 0;
            for (int i = 0; i < resolutions.Count; i++)
            {
                if (resolutions[i].width == 1920 && resolutions[i].height == 1080)
                {
                    resolution1080Index = i;
                    break;
                }
            }
            resolutionDropdown.value = resolution1080Index;
        }
        
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = true;
        if (vsyncToggle != null)
            vsyncToggle.isOn = true;

        // Apply defaults
        ApplySettings();
    }

    void Back()
    {
        // Switch back to main menu through MainMenu script
        if (mainMenuScript != null)
        {
            mainMenuScript.CloseSettings();
        }
        else
        {
            // Fallback: direct panel switching (if MainMenu not found)
            if (settingsPanel != null)
                settingsPanel.SetActive(false);
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(true);
        }
    }
}
