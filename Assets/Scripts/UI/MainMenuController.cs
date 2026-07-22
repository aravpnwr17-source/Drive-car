using UnityEngine;
using TMPro;

/// <summary>
/// Main menu controller - Handle main menu UI and game start
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private TextMeshProUGUI versionText;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider difficultySlider;

    private void Start()
    {
        Time.timeScale = 1f;
        ShowMainMenu();
        if (versionText != null)
            versionText.text = "v0.1.0-alpha";
    }

    /// <summary>
    /// Start the game
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Starting game...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGameScene");
    }

    /// <summary>
    /// Show main menu
    /// </summary>
    public void ShowMainMenu()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    /// <summary>
    /// Show settings menu
    /// </summary>
    public void ShowSettings()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    /// <summary>
    /// Set master volume
    /// </summary>
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        Debug.Log($"Volume set to: {volume}");
    }

    /// <summary>
    /// Set game difficulty
    /// </summary>
    public void SetDifficulty(float difficulty)
    {
        // Easy: 0-0.33, Normal: 0.33-0.66, Hard: 0.66-1.0
        if (difficulty < 0.33f)
            Debug.Log("Difficulty: Easy");
        else if (difficulty < 0.66f)
            Debug.Log("Difficulty: Normal");
        else
            Debug.Log("Difficulty: Hard");
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
