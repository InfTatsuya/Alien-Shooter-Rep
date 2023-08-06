using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTaskGroup_Patrolling : BTTask_Group
{
    private float acceptableDistance = 1.5f;

    public BTTaskGroup_Patrolling(BehaviorTree behaviourTree, float acceptableDistance) : base(behaviourTree)
    {
        this.acceptableDistance = acceptableDistance;
    }

    protected override void ConstructTree(out BTNode root)
    {
        Sequencer patrollingSequence = new Sequencer();
        BTTask_GetNextPatrolPoint getNextPatrolPoint = new BTTask_GetNextPatrolPoint(behaviorTree);
        BTTask_MoveToLocation moveTo = new BTTask_MoveToLocation(behaviorTree, StringCollector.patrolPointString, acceptableDistance);
        BTTask_Wait waitAtPatrolPoint = new BTTask_Wait(2f);

        patrollingSequence.AddChild(getNextPatrolPoint);
        patrollingSequence.AddChild(moveTo);
        patrollingSequence.AddChild(waitAtPatrolPoint);

        root = patrollingSequence;
    }
}
