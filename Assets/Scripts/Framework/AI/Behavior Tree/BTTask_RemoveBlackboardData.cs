using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_RemoveBlackboardData : BTNode
{
    private BehaviorTree behaviorTree;
    private string keyToRemove;

    public BTTask_RemoveBlackboardData(BehaviorTree behaviorTree, string keyToRemove)
    {
        this.behaviorTree = behaviorTree;
        this.keyToRemove = keyToRemove;
    }

    protected override NodeResult Execute()
    {
        if(behaviorTree != null && behaviorTree.BlackBoard != null)
        {
            behaviorTree.BlackBoard.RemoveBlackboardData(keyToRemove);
            return NodeResult.Success;
        }

        return NodeResult.Failure;
    }
}
