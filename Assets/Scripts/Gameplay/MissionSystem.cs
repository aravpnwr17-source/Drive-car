using UnityEngine;

/// <summary>
/// Manages game missions and objectives
/// </summary>
public class MissionSystem : MonoBehaviour
{
    [System.Serializable]
    public class Mission
    {
        public int id;
        public string missionName;
        public string description;
        public MissionType type;
        public float reward;
        public bool isCompleted;
        public bool isActive;
    }

    public enum MissionType { Delivery, Racing, Stunt, Rescue, Exploration }

    [SerializeField] private Mission[] availableMissions = new Mission[10];
    [SerializeField] private HUDManager hudManager;

    private int currentMissionIndex = -1;
    private float missionProgress = 0f;

    private void Start()
    {
        InitializeMissions();
    }

    /// <summary>
    /// Initialize all available missions
    /// </summary>
    private void InitializeMissions()
    {
        // Create sample missions
        if (availableMissions.Length > 0)
        {
            availableMissions[0] = new Mission
            {
                id = 1,
                missionName = "Package Delivery",
                description = "Deliver package to the warehouse",
                type = MissionType.Delivery,
                reward = 500f,
                isCompleted = false,
                isActive = false
            };

            availableMissions[1] = new Mission
            {
                id = 2,
                missionName = "City Race",
                description = "Race through the city streets",
                type = MissionType.Racing,
                reward = 1000f,
                isCompleted = false,
                isActive = false
            };
        }
    }

    /// <summary>
    /// Start a mission
    /// </summary>
    public void StartMission(int missionIndex)
    {
        if (missionIndex < 0 || missionIndex >= availableMissions.Length) return;

        if (currentMissionIndex != -1 && !availableMissions[currentMissionIndex].isCompleted)
        {
            Debug.LogWarning("Complete current mission before starting another!");
            return;
        }

        currentMissionIndex = missionIndex;
        availableMissions[missionIndex].isActive = true;
        missionProgress = 0f;

        if (hudManager != null)
            hudManager.UpdateMission(availableMissions[missionIndex].missionName);

        Debug.Log($"Mission Started: {availableMissions[missionIndex].missionName}");
    }

    /// <summary>
    /// Update mission progress
    /// </summary>
    public void UpdateMissionProgress(float progressAmount)
    {
        if (currentMissionIndex == -1) return;

        missionProgress = Mathf.Clamp01(missionProgress + progressAmount);

        if (missionProgress >= 1f)
        {
            CompleteMission();
        }
    }

    /// <summary>
    /// Complete current mission
    /// </summary>
    private void CompleteMission()
    {
        if (currentMissionIndex == -1) return;

        Mission completedMission = availableMissions[currentMissionIndex];
        completedMission.isCompleted = true;
        completedMission.isActive = false;

        if (hudManager != null)
            hudManager.ShowNotification($"Mission Complete! +{completedMission.reward} XP");

        Debug.Log($"Mission Completed: {completedMission.missionName} | Reward: {completedMission.reward}");

        currentMissionIndex = -1;
        missionProgress = 0f;
    }

    /// <summary>
    /// Fail current mission
    /// </summary>
    public void FailMission()
    {
        if (currentMissionIndex == -1) return;

        Mission failedMission = availableMissions[currentMissionIndex];
        failedMission.isActive = false;

        if (hudManager != null)
            hudManager.ShowNotification("Mission Failed!");

        Debug.Log($"Mission Failed: {failedMission.missionName}");

        currentMissionIndex = -1;
        missionProgress = 0f;
    }

    /// <summary>
    /// Get all available missions
    /// </summary>
    public Mission[] GetAvailableMissions()
    {
        return availableMissions;
    }

    /// <summary>
    /// Get current mission
    /// </summary>
    public Mission GetCurrentMission()
    {
        if (currentMissionIndex == -1) return null;
        return availableMissions[currentMissionIndex];
    }

    /// <summary>
    /// Get current mission progress (0-1)
    /// </summary>
    public float GetMissionProgress()
    {
        return missionProgress;
    }
}
