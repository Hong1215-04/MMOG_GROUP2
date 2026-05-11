using UnityEngine;
using UnityEngine.SceneManagement; // For scene switching
using UnityEngine.UI; // For button binding

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button settingButton;
    public Button quitButton;

    // References to UI panels
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject characterSelectPanel;

    void Start()
    {
        // Bind button events
        startButton.onClick.AddListener(StartGame);
        settingButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
    }

    void StartGame()
    {
        SFXManager.Instance?.PlayButtonClick();
        mainMenuPanel.SetActive(false);
        characterSelectPanel.SetActive(true);
        BGMManager.Instance?.PlayCharacterSelectBGM();
    }


    void OpenSettings()
    {
        SFXManager.Instance?.PlayButtonClick();
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        SFXManager.Instance?.PlayButtonClick();
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        BGMManager.Instance?.PlayMenuBGM();
    }

    void QuitGame()
    {
        SFXManager.Instance?.PlayButtonClick();
        Debug.Log("Quit Game");
        Application.Quit();

        // In editor, won't quit, add this for testing:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
