using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Ability/Speed Boost")]
public class Ability_SpeedBoost : Ability
{
    [SerializeField] float boostAmt = 20f;
    [SerializeField] float boostDuration = 5f;

    private Player player;

    public override void ActivateAbility()
    {
        if(!CommitAbility()) return;

        player = AbilityComp.GetComponent<Player>();

        if(player == null) return;  
        
        player.AddMoveSpeed(boostAmt);

        AbilityComp.StartCoroutine(ResetSpeed());
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(boostDuration);

        player.AddMoveSpeed(-boostAmt);
    }
}
