using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    public enum GameState { Menu, Playing, Paused, GameOver }
    public GameState currentState;

    // Player data
    public int player1Health = 100;
    public int player2Health = 100;
    public int player1Score = 0;
    public int player2Score = 0;

    // Round settings
    public float roundTime = 99f; // seconds
    private float currentTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentState = GameState.Menu;
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 || player1Health <= 0 || player2Health <= 0)
            {
                EndGame();
            }
        }

        // Pause with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void StartGame()
    {
        currentState = GameState.Playing;
        currentTime = roundTime;
        // Load game scene or initialize
        SceneManager.LoadScene("GameScene");
    }

    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0; // Pause time
        }
        else if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = 1;
        }
    }

    public void EndGame()
    {
        currentState = GameState.GameOver;
        // Determine winner
        if (player1Health > player2Health)
        {
            Debug.Log("Player 1 Wins!");
        }
        else if (player2Health > player1Health)
        {
            Debug.Log("Player 2 Wins!");
        }
        else
        {
            Debug.Log("Draw!");
        }
        // Return to menu
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetGame()
    {
        player1Health = 100;
        player2Health = 100;
        player1Score = 0;
        player2Score = 0;
        currentTime = roundTime;
    }
}