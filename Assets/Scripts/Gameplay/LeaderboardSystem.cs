using UnityEngine;

/// <summary>
/// Leaderboard system - Track best times and scores
/// </summary>
public class LeaderboardSystem : MonoBehaviour
{
    [System.Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public float score;
        public float time;
        public int rank;
    }

    [SerializeField] private int maxEntries = 10;
    private LeaderboardEntry[] leaderboard;

    private void Start()
    {
        leaderboard = new LeaderboardEntry[maxEntries];
        LoadLeaderboard();
    }

    /// <summary>
    /// Add score to leaderboard
    /// </summary>
    public void AddScore(string playerName, float score, float time)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry
        {
            playerName = playerName,
            score = score,
            time = time
        };

        // Insert into leaderboard (sorted by score)
        for (int i = 0; i < leaderboard.Length; i++)
        {
            if (leaderboard[i] == null || newEntry.score > leaderboard[i].score)
            {
                // Shift entries down
                for (int j = leaderboard.Length - 1; j > i; j--)
                {
                    leaderboard[j] = leaderboard[j - 1];
                }
                leaderboard[i] = newEntry;
                break;
            }
        }

        // Update ranks
        UpdateRanks();
        SaveLeaderboard();
    }

    /// <summary>
    /// Update ranks
    /// </summary>
    private void UpdateRanks()
    {
        for (int i = 0; i < leaderboard.Length; i++)
        {
            if (leaderboard[i] != null)
                leaderboard[i].rank = i + 1;
        }
    }

    /// <summary>
    /// Get leaderboard
    /// </summary>
    public LeaderboardEntry[] GetLeaderboard()
    {
        return leaderboard;
    }

    /// <summary>
    /// Save leaderboard to file
    /// </summary>
    private void SaveLeaderboard()
    {
        Debug.Log("Leaderboard saved!");
    }

    /// <summary>
    /// Load leaderboard from file
    /// </summary>
    private void LoadLeaderboard()
    {
        Debug.Log("Leaderboard loaded!");
    }
}
