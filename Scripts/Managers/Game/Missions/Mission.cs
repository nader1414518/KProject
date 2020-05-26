using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new mission", menuName = "Mission")]
public class Mission : ScriptableObject
{
    public new string name;
    public string description;
    public int xp_reward;
    public int money_reward;
    public string status;
    public int mission_level;
    public Sprite icon;
    public bool is_collected = false;

    public Mission(string mission_name, string description, int xp_reward, int money_reward, string status)
    {
        name = mission_name;
        this.description = description;
        this.xp_reward = xp_reward;
        this.money_reward = money_reward;
        this.status = status;
    }
    public Mission(string mission_name, string description, int xp_reward, int money_reward, string status, int mission_level)
    {
        name = mission_name;
        this.description = description;
        this.xp_reward = xp_reward;
        this.money_reward = money_reward;
        this.status = status;
        this.mission_level = mission_level;
    }
    public Mission()
    {
        this.name = "";
        this.description = "";
        this.xp_reward = 0;
        this.money_reward = 0;
        this.status = "";
        this.icon = null;
    }
}
