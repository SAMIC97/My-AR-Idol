using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MissionCheck : MonoBehaviour
{
    public GameObject panelOutfitLock;
    public GameObject panelMissionList;
    public bool firstModel = true;
    public bool missionsDone = false;

    private const string MissionCompletedPrefix = "MissionCompleted_";
    private const int TotalMissions = 3;
    private bool[] missionCompleted;
    private Image[] missionImages;//Images of missions

    [SerializeField] private GameData gameData;


    private void Start()
    {
        missionCompleted = gameData.MissionCompleted;

        // Get all child Image components of panelMissionList excluding buttons and parent Image
        missionImages = panelMissionList.GetComponentsInChildren<Image>(false)
            .Where(img => img.transform.parent == panelMissionList.transform && img.GetComponent<Button>() == null)
            .ToArray();

        // Load mission completion status
        LoadMissionCompletion();

        // Check unlock conditions
        CheckUnlockOutfit();
    }

    private void LoadMissionCompletion()
    {
        for (int i = 0; i < TotalMissions; i++)
        {
            if (missionCompleted[i])
            {
                // Update UI for completed missions
                ChangeColorMission(i);
            }
        }
    }

    public void OnGoalComplete(string trigger)
    {
        int missionIndex = -1;
        if (trigger == "date_goal_completed")
        {
            missionIndex = 0;
            ChangeColorMission(missionIndex);
        }
        else if (trigger == "group_gretting_goal_completed")
        {
            missionIndex = 1;
            ChangeColorMission(missionIndex);
        }

        if (missionIndex != -1)
        {
            if (!missionCompleted[missionIndex])
            {
                CompleteMission(missionIndex);
            }
        }
    }

    public void CompleteMission(int missionIndex)
    {
        if (!gameData.MissionCompleted[missionIndex]) // Check if the mission is not already completed
        {
            gameData.MissionCompleted[missionIndex] = true;

            gameData.MissionsDone++; // Increment the count only if the mission is not already completed

            CheckUnlockOutfit();
        }
    }

    public void CheckUnlockOutfit()
    {
        if (gameData.MissionsDone >= TotalMissions || firstModel)
        {
            UnlockOutfit();
        }
        else if (!firstModel || gameData.MissionsDone <= TotalMissions)
        {
            LockOutfit();
        }
    }

    private void LockOutfit()
    {
        panelOutfitLock.SetActive(true);
        missionsDone = false;
    }

    private void UnlockOutfit()
    {
        panelOutfitLock.SetActive(false);
        missionsDone = true;
    }

    public void ChangeColorMission(int missionIndex)
    {
        if (missionIndex >= 0 && missionIndex < missionImages.Length)
        {
            missionImages[missionIndex].color = Color.green;
        }
    }

    public void ResetMissionCompletion()
    {
        for (int i = 0; i < TotalMissions; i++)
        {
            missionCompleted[i] = false;
        }
        gameData.MissionCompleted = missionCompleted;

        gameData.MissionsDone = 0;
        CheckUnlockOutfit();
    }
}
