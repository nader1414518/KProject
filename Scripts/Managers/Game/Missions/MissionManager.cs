using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public List<GameObject> missionsSlotList;
    public List<Mission> missionsList;
    public List<Mission> currentMissions = new List<Mission>();

    void OnEnable()
    {
        // Retrieve missions for player username 
        //RetrieveMissionsFromTheDB();
        // Update the UI with description for each mission 
        // Hide missions slots 
        HideMissionsSlots();
        // Show Mission for current level 
        DisplayLevelMission();
    }
    
    void HideMissionsSlots()
    {
        for (int i = 0; i < missionsSlotList.Count; i++)
        {
            missionsSlotList[i].SetActive(false);
        }
    }

    // (Do not use unless you want to save mission in the database)
    void RetrieveMissionsFromTheDB()
    {
        if (game_globals.GetMissionsForPlayer() != null)
        {
            //for (int i = 0; i < game_globals.GetMissionsForPlayer().Count; i++)
            //{
            //    missionsArray[i] = game_globals.GetMissionsForPlayer()[i];
            //}
            missionsList = game_globals.GetMissionsForPlayer();
        }
    }

    void DisplayLevelMission()
    {
        // Get player level
        int player_level = game_globals.GetPlayerCharacterLevel();

        // Show missions for this level 
        for (int i = 0; i < missionsList.Count; i++)
        {
            if (missionsList[i].mission_level == player_level && missionsList[i].status != "Completed")
            {
                // Add mission to current missions list 
                currentMissions.Add(missionsList[i]);
            }
        }

        // Update the UI 
        DisplayUIMissions();
    }
    
    // Set the UI for the missions list on the missions panel 
    void DisplayUIMissions()
    {
        if (currentMissions != null)
        {
            for (int i = 0; i < currentMissions.Count; i++)
            {
                // Assign text of the mission
                missionsSlotList[i].SetActive(true);
                missionsSlotList[i].GetComponentInChildren<Text>().text = currentMissions[i].name + "\n   " + currentMissions[i].description;
                missionsSlotList[i].GetComponentInChildren<Button>().onClick.AddListener(delegate { CollectMissionRewardBtnCallback(currentMissions[i]); });
                //missionsSlotList[i].GetComponentInChildren<Button>().onClick.AddListener(() => CollectMissionRewardBtnCallback(currentMissions[i]));
            }
            Debug.Log("The count of added missions: " + currentMissions.Count);
        }
    }

    // Callback of the collect mission reward btn 
    void CollectMissionRewardBtnCallback(Mission mission)
    {
        Debug.Log("The count of added missions: " + currentMissions.Count);
        if (mission != null)
        {
            if (mission.status == "Completed")
            {
                // The player completed the mission 
                for (int i = 0; i < missionsList.Count; i++)
                {
                    if (mission.name == missionsList[i].name)
                    {
                        missionsList[i].status = "Completed";
                    }
                }
                // Add mission xp reward
                game_db_manager.IncreaseCharacterXP(game_globals.playerUsername, game_globals.playerCharacterName, mission.xp_reward);
                // Add mission money reward
                game_db_manager.IncreaseCharacterMoney(game_globals.playerUsername, game_globals.playerCharacterName, mission.money_reward);
                // Mark as collected 
                mission.is_collected = true;
                //currentMissions.Remove(mission);
            }
            else
            {
                // Player did not complete the mission 
                Debug.Log("Complete the mission first ... ");
            }
        }
    }
}
