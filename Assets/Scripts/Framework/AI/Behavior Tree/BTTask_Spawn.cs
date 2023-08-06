using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_Spawn : BTNode
{
    SpawnComponent spawnComponent;

    public BTTask_Spawn(BehaviorTree behaviorTree)
    {
        spawnComponent = behaviorTree.GetComponent<SpawnComponent>();
    }

    protected override NodeResult Execute()
    {
        if(spawnComponent == null || !spawnComponent.StartSpawn())
        {
            return NodeResult.Failure;
        }

        return NodeResult.Success;
    }
}
