using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_RotateTowardTarget : BTNode
{
    private BehaviorTree behaviorTree;
    private string key;
    private float acceptableDegrees;
    private GameObject target;
    private IBehaviorTreeInterface behaviorTreeInterface;

    public BTTask_RotateTowardTarget(BehaviorTree behaviorTree, string key, float acceptableDegrees)
    {
        this.behaviorTree = behaviorTree;
        this.key = key;
        this.acceptableDegrees = acceptableDegrees;
        behaviorTreeInterface = behaviorTree.BehaviorTreeInterface; 
    }

    protected override NodeResult Execute()
    {
        if(behaviorTree == null || behaviorTree.BlackBoard == null)
        {
            return NodeResult.Failure;
        }

        if(behaviorTreeInterface == null)
        {
            return NodeResult.Failure;
        }

        if(!behaviorTree.BlackBoard.GetBlackboardData<GameObject>(key, out target))
        {
            return NodeResult.Failure;
        }

        behaviorTree.BlackBoard.onBlackboardValueChange += BlackBoard_onBlackboardValueChange;

        if (IsInAcceptableDegrees())
        {
            return NodeResult.Success;
        }
        else
        {
            return NodeResult.InProgress;
        }
    }

    private void BlackBoard_onBlackboardValueChange(string key, object value)
    {
        if (key != this.key) return;

        target = (GameObject)value; 
    }

    protected override NodeResult Update()
    {
        if (target == null) return NodeResult.Failure;

        if(IsInAcceptableDegrees())
        {
            return NodeResult.Success;
        }

        behaviorTreeInterface.RotateToward(target);
        return NodeResult.InProgress;
    }

    protected override void End()
    {
        base.End();

        behaviorTree.BlackBoard.onBlackboardValueChange -= BlackBoard_onBlackboardValueChange;
    }

    private bool IsInAcceptableDegrees()
    {
        Vector3 aimDir = (target.transform.position - behaviorTree.transform.position).normalized;
        Vector3 dir = behaviorTree.transform.forward;

        float degrees = Vector3.Angle(dir, aimDir);

        return degrees <= acceptableDegrees;
    }
}
