using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviorTree : BehaviorTree
{
    [SerializeField] float spawnCooldownDuration = 3f;

    protected override void ConstructTree(out BTNode rootNode)
    {
        BTTask_Spawn spawn = new BTTask_Spawn(this);
        CooldownDecorator cooldownDecorator = 
            new CooldownDecorator(this, spawn, spawnCooldownDuration);
        BlackboardDecorator blackboardDecorator =
            new BlackboardDecorator(this, cooldownDecorator, StringCollector.targetString,
            BlackboardDecorator.RunCondition.KeyExists, BlackboardDecorator.NotifyRule.RunConditionChange,
            BlackboardDecorator.NotifyAbort.both);

        rootNode = blackboardDecorator;
    }
}
