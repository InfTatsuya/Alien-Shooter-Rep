using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopper : Enemy
{
    [SerializeField] TriggerDamageComponent triggerDamageComponent;


    protected override void Start()
    {
        base.Start();

        triggerDamageComponent.SetupTeamInterface(this.gameObject);
    }

    public override void AttackTarget(GameObject target)
    {
        Anim.SetTrigger(StringCollector.attackAnim);
    }

    public void AttackPoint()
    {
        triggerDamageComponent.SetDamageEnabled(true);
    }

    public void AttackEnd()
    {
        triggerDamageComponent.SetDamageEnabled(false);
    }
}
