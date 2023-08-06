using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class BTTask_AttackTarget : BTNode
{
    private BehaviorTree behaviorTree;
    private string key;
    private GameObject target;
    private float timer;

    public BTTask_AttackTarget(BehaviorTree behaviorTree, string key)
    {
        this.behaviorTree = behaviorTree;
        this.key = key;
    }

    protected override NodeResult Execute()
    {
        if( behaviorTree == null || 
            behaviorTree.BlackBoard == null || 
            !behaviorTree.BlackBoard.GetBlackboardData<GameObject>(key, out target))
        {
            return NodeResult.Failure;
        }

        IBehaviorTreeInterface behaviorTreeInterface = behaviorTree.BehaviorTreeInterface;
        if (behaviorTreeInterface == null) return NodeResult.Failure;

        behaviorTreeInterface.AttackTarget(target);
        timer = 0.7f;
        return NodeResult.InProgress;
    }

    protected override NodeResult Update()
    {
        timer -= Time.deltaTime;

        if(timer >= 0f)
        {
            return NodeResult.InProgress;
        }

        return NodeResult.Success;
    }
}
