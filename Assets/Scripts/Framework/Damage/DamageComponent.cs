using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageComponent : MonoBehaviour, ITeamInterface
{
    [SerializeField] protected bool damageFriendly;
    [SerializeField] protected bool damageEnemy;
    [SerializeField] protected bool damageNeutral;

    private ITeamInterface teamInterface;

    public int GetTeamID()
    {
        if(teamInterface != null)
        {
            return teamInterface.GetTeamID();
        }

        return -1;
    }


    public void SetupTeamInterface(GameObject source)
    {
        teamInterface = source.GetComponent<ITeamInterface>();
    }

    public bool ShouldDamage(GameObject other)
    {
        if (teamInterface == null) return false;

        TeamRelation relation = teamInterface.GetRelationToward(other);

        if(damageFriendly && relation == TeamRelation.Friendly)
        {
            return true;
        }

        if (damageEnemy && relation == TeamRelation.Enemy)
        {
            return true;
        }

        if (damageNeutral && relation == TeamRelation.Neutral)
        {
            return true;
        }

        return false;
    }
}
