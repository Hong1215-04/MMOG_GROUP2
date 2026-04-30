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

    void Start()
    {
        // Bind button events
        startButton.onClick.AddListener(StartGame);
        settingButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
    }

    void StartGame()
    {
        // Assuming the main game scene is called "GameScene"
        SceneManager.LoadScene("GameScene");
    }

    void OpenSettings()
    {
        // Switch to settings panel
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        // Switch back to main menu panel from settings
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();

        // In editor, won't quit, add this for testing:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
