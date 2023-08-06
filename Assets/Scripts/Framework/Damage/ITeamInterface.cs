using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamRelation
{
    Friendly,
    Enemy,
    Neutral
}

public interface ITeamInterface 
{
    public int GetTeamID() { return -1; }

    public TeamRelation GetRelationToward(GameObject other)
    {
        ITeamInterface otherTeamInterface = other.GetComponent<ITeamInterface>();   

        if (otherTeamInterface == null)
        {
            return TeamRelation.Neutral;
        }

        if(otherTeamInterface.GetTeamID() == GetTeamID())
        {
            return TeamRelation.Friendly;
        }
        else if(otherTeamInterface.GetTeamID() == -1 || GetTeamID() == -1)
        {
            return TeamRelation.Neutral;
        }
        else
        {
            return TeamRelation.Enemy;
        }
    }
}
