using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTTask_MoveToLocation : BTNode
{
    private NavMeshAgent agent;
    private string locationKey;
    private Vector3 location;
    private float acceptableDistance = 1f;

    private BehaviorTree behaviorTree;

    public BTTask_MoveToLocation(BehaviorTree behaviorTree, string key, float acceptableDist)
    {
        this.locationKey = key;
        this.acceptableDistance = acceptableDist;
        this.behaviorTree = behaviorTree;
    }

    protected override NodeResult Execute()
    {
        Blackboard blackboard = behaviorTree.BlackBoard;
        if (blackboard == null || !blackboard.GetBlackboardData<Vector3>(locationKey, out location))
        {
            return NodeResult.Failure;
        }

        agent = behaviorTree.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            return NodeResult.Failure;
        }

        if (IsCloseToTheLocation())
        {
            return NodeResult.Success;
        }

        agent.SetDestination(location);
        agent.isStopped = false;
        return NodeResult.InProgress;
    }

    protected override NodeResult Update()
    {
        if (IsCloseToTheLocation())
        {
            agent.isStopped = true;
            agent.ResetPath();

            return NodeResult.Success;
        }

        return NodeResult.InProgress;
    }

    private bool IsCloseToTheLocation()
        => Vector3.Distance(behaviorTree.transform.position, location) <= acceptableDistance;

    protected override void End()
    {
        base.End();

        agent.isStopped = true;
        agent.ResetPath();
    }

}
