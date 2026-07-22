using UnityEngine;

/// <summary>
/// Race system - Racing missions and time trials
/// </summary>
public class RaceSystem : MonoBehaviour
{
    [System.Serializable]
    public class RaceTrack
    {
        public int id;
        public string trackName;
        public float trackLength;
        public float recordTime;
        public int laps;
        public float reward;
    }

    [SerializeField] private RaceTrack[] raceTracks = new RaceTrack[5];
    private int currentRaceIndex = -1;
    private float currentRaceTime = 0f;
    private int currentLap = 1;
    private bool isRacing = false;

    private void Start()
    {
        InitializeRaces();
    }

    private void Update()
    {
        if (isRacing)
            currentRaceTime += Time.deltaTime;
    }

    /// <summary>
    /// Initialize available race tracks
    /// </summary>
    private void InitializeRaces()
    {
        if (raceTracks.Length > 0)
        {
            raceTracks[0] = new RaceTrack
            {
                id = 1,
                trackName = "City Circuit",
                trackLength = 10f,
                recordTime = 300f,
                laps = 3,
                reward = 1000f
            };
        }
    }

    /// <summary>
    /// Start a race
    /// </summary>
    public void StartRace(int raceIndex)
    {
        if (raceIndex < 0 || raceIndex >= raceTracks.Length) return;

        currentRaceIndex = raceIndex;
        currentRaceTime = 0f;
        currentLap = 1;
        isRacing = true;

        Debug.Log($"Race Started: {raceTracks[raceIndex].trackName}");
    }

    /// <summary>
    /// Finish current lap
    /// </summary>
    public void FinishLap()
    {
        if (!isRacing) return;

        RaceTrack currentTrack = raceTracks[currentRaceIndex];
        currentLap++;

        if (currentLap > currentTrack.laps)
            FinishRace();
        else
            Debug.Log($"Lap {currentLap}/{currentTrack.laps}");
    }

    /// <summary>
    /// Finish the race
    /// </summary>
    public void FinishRace()
    {
        if (!isRacing) return;

        isRacing = false;
        RaceTrack track = raceTracks[currentRaceIndex];

        if (currentRaceTime < track.recordTime)
        {
            Debug.Log($"New Record! Time: {currentRaceTime:F2}s | Reward: {track.reward}");
        }
        else
        {
            Debug.Log($"Race Complete! Time: {currentRaceTime:F2}s | Reward: {track.reward}");
        }
    }
}
