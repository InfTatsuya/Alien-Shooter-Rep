using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_GetNextPatrolPoint : BTNode
{
    private BehaviorTree behaviorTree;
    private PatrollingComponent patrollingComp;

    public BTTask_GetNextPatrolPoint(BehaviorTree behaviorTree)
    {
        this.behaviorTree = behaviorTree;
        patrollingComp = behaviorTree.GetComponent<PatrollingComponent>();
    }

    protected override NodeResult Execute()
    {
        if(patrollingComp != null && patrollingComp.GetNextPatrolPoint(out Vector3 point))
        {
            behaviorTree.BlackBoard.SetOrAddData(StringCollector.patrolPointString, point);
            return NodeResult.Success;
        }

        return NodeResult.Failure;
    }
}
