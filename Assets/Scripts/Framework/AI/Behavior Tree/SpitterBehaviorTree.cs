using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterBehaviorTree : BehaviorTree
{
    [SerializeField] float acceptableDistance = 5f;
    [SerializeField] float acceptableDegrees = 10f;
    [SerializeField] float attackCooldownDuration = 2f;

    protected override void ConstructTree(out BTNode rootNode)
    {
        BTTaskGroup_AttackTarget attackTarget = 
            new BTTaskGroup_AttackTarget(this, acceptableDistance, acceptableDegrees, attackCooldownDuration);

        BTTaskGroup_CheckLastSeenPosition checkLastSeenPos =
            new BTTaskGroup_CheckLastSeenPosition(this, acceptableDistance);

        BTTaskGroup_Patrolling patrolling =
            new BTTaskGroup_Patrolling(this, acceptableDistance);

        Selector rootSelector = new Selector();

        rootSelector.AddChild(attackTarget);
        rootSelector.AddChild(checkLastSeenPos);
        rootSelector.AddChild(patrolling);

        rootNode = rootSelector;

    }
}
