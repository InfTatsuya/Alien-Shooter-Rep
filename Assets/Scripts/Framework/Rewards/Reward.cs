using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Reward 
{
    public int healthReward;
    public int staminaReward;
    public int creditReward;
}

public interface IRewardListener
{
    public void Reward(Reward reward);
}

