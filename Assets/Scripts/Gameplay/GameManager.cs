using UnityEngine;

/// <summary>
/// Game manager that handles overall game flow and state
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CarController playerCar;
    [SerializeField] private MissionSystem missionSystem;
    [SerializeField] private HUDManager hudManager;
    [SerializeField] private WeatherSystem weatherSystem;

    private GameState currentGameState = GameState.Playing;
    private float totalPlayTime = 0f;
    private int totalXP = 0;
    private int playerLevel = 1;

    public enum GameState { Playing, Paused, GameOver, MainMenu }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ResumeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.Playing)
                PauseGame();
            else if (currentGameState == GameState.Paused)
                ResumeGame();
        }

        if (currentGameState == GameState.Playing)
            totalPlayTime += Time.deltaTime;
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame()
    {
        currentGameState = GameState.Paused;
        Time.timeScale = 0f;
        Debug.Log("Game Paused");
    }

    /// <summary>
    /// Resume the game
    /// </summary>
    public void ResumeGame()
    {
        currentGameState = GameState.Playing;
        Time.timeScale = 1f;
        Debug.Log("Game Resumed");
    }

    /// <summary>
    /// End the game
    /// </summary>
    public void GameOver()
    {
        currentGameState = GameState.GameOver;
        Time.timeScale = 0f;
        Debug.Log($"Game Over! Total Play Time: {totalPlayTime:F0}s | Total XP: {totalXP}");
    }

    /// <summary>
    /// Add XP to player
    /// </summary>
    public void AddXP(int xpAmount)
    {
        totalXP += xpAmount;
        
        // Level up every 1000 XP
        int newLevel = (totalXP / 1000) + 1;
        if (newLevel > playerLevel)
        {
            playerLevel = newLevel;
            if (hudManager != null)
                hudManager.ShowNotification($"Level Up! You are now level {playerLevel}");
            Debug.Log($"Level Up! New Level: {playerLevel}");
        }
    }

    /// <summary>
    /// Complete a mission with reward
    /// </summary>
    public void CompleteMission(string missionName, float xpReward)
    {
        AddXP((int)xpReward);
        if (hudManager != null)
            hudManager.ShowNotification($"{missionName} Complete! +{xpReward} XP");
    }

    /// <summary>
    /// Get current game state
    /// </summary>
    public GameState GetGameState()
    {
        return currentGameState;
    }

    /// <summary>
    /// Get player level
    /// </summary>
    public int GetPlayerLevel()
    {
        return playerLevel;
    }

    /// <summary>
    /// Get total XP
    /// </summary>
    public int GetTotalXP()
    {
        return totalXP;
    }

    /// <summary>
    /// Get total play time in seconds
    /// </summary>
    public float GetTotalPlayTime()
    {
        return totalPlayTime;
    }
}
