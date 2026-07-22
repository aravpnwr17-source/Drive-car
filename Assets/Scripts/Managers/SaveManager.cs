using UnityEngine;

/// <summary>
/// Save/Load manager - Handle game data persistence
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [System.Serializable]
    public class GameData
    {
        public int playerLevel = 1;
        public int totalXP = 0;
        public float totalPlayTime = 0f;
        public int moneyCurrency = 0;
    }

    private GameData gameData;
    private string savePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Application.persistentDataPath + "/gamedata.json";
        LoadGame();
    }

    /// <summary>
    /// Save game data to file
    /// </summary>
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(gameData, true);
        System.IO.File.WriteAllText(savePath, json);
        Debug.Log("Game saved!");
    }

    /// <summary>
    /// Load game data from file
    /// </summary>
    public void LoadGame()
    {
        if (System.IO.File.Exists(savePath))
        {
            string json = System.IO.File.ReadAllText(savePath);
            gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game loaded!");
        }
        else
        {
            gameData = new GameData();
            Debug.Log("New game started!");
        }
    }

    /// <summary>
    /// Get current game data
    /// </summary>
    public GameData GetGameData() => gameData;

    /// <summary>
    /// Add currency
    /// </summary>
    public void AddCurrency(int amount)
    {
        gameData.moneyCurrency += amount;
    }
}
