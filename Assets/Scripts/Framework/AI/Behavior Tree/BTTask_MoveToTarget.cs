using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTTask_MoveToTarget : BTNode
{
    private NavMeshAgent agent;
    private string targetKey = "Target";
    private GameObject target;
    private float acceptableDistance = 1f;

    private BehaviorTree behaviorTree;

    public BTTask_MoveToTarget(BehaviorTree behaviorTree, string targetKey, float acceptableDist)
    {
        this.targetKey = targetKey;
        this.acceptableDistance = acceptableDist;
        this.behaviorTree = behaviorTree;
    }

    protected override NodeResult Execute()
    {
        Blackboard blackboard = behaviorTree.BlackBoard;
        if(blackboard == null || !blackboard.GetBlackboardData<GameObject>(targetKey, out target))
        {
            return NodeResult.Failure;
        }

        blackboard.onBlackboardValueChange += Blackboard_onBlackboardValueChange;

        agent = behaviorTree.GetComponent<NavMeshAgent>();
        if(agent == null)
        {
            return NodeResult.Failure;
        }

        if (IsTargetInAcceptableDistance())
        {
            return NodeResult.Success;
        }

        agent.SetDestination(target.transform.position);
        agent.isStopped = false;
        return NodeResult.InProgress;
    }

    private void Blackboard_onBlackboardValueChange(string key, object value)
    {
        if(key == targetKey)
        {
            target = (GameObject)value;
        }
    }

    protected override NodeResult Update()
    {
        if(target == null)
        {
            agent.isStopped = true;
            return NodeResult.Failure;
        }

        agent.SetDestination(target.transform.position);
        if(IsTargetInAcceptableDistance() )
        {
            agent.isStopped = true;
            agent.ResetPath();

            return NodeResult.Success;
        }

        return NodeResult.InProgress;
    }

    private bool IsTargetInAcceptableDistance() 
        => Vector3.Distance(behaviorTree.transform.position, target.transform.position) <= acceptableDistance;

    protected override void End()
    {
        base.End();

        behaviorTree.BlackBoard.onBlackboardValueChange -= Blackboard_onBlackboardValueChange;
        agent.isStopped = true;
        agent.ResetPath();
    }
}
