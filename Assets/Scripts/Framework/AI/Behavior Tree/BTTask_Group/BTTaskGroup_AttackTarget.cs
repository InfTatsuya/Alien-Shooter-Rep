using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTaskGroup_AttackTarget : BTTask_Group
{
    private float acceptableDistance = 1.5f;
    private float acceptableDegrees = 10f;
    private float attackCooldownDuration = 0f;

    public BTTaskGroup_AttackTarget(BehaviorTree behaviourTree, float acceptDistance, float acceptDegrees, float attackCooldownDuration) : base(behaviourTree)
    {
        this.acceptableDistance = acceptDistance;
        this.acceptableDegrees = acceptDegrees;
        this.attackCooldownDuration = attackCooldownDuration;
    }

    protected override void ConstructTree(out BTNode root)
    {
        Sequencer attackSequencer = new Sequencer();

        BTTask_MoveToTarget moveToTarget =
            new BTTask_MoveToTarget(behaviorTree, StringCollector.targetString, acceptableDistance);
        BTTask_RotateTowardTarget rotateTowardTarget =
            new BTTask_RotateTowardTarget(behaviorTree, StringCollector.targetString, acceptableDegrees);
        
        BTTask_AttackTarget attackTarget =
            new BTTask_AttackTarget(behaviorTree, StringCollector.targetString);
        CooldownDecorator cooldownDecorator =
            new CooldownDecorator(behaviorTree, attackTarget, attackCooldownDuration, false);

        //BTTask_Wait waitToRepeatAttack = new BTTask_Wait(1f);

        attackSequencer.AddChild(moveToTarget);
        attackSequencer.AddChild(rotateTowardTarget);
        attackSequencer.AddChild(cooldownDecorator);

        BlackboardDecorator checkHavingTargetDecorator =
            new BlackboardDecorator(behaviorTree,
                                    attackSequencer,
                                    StringCollector.targetString,
                                    BlackboardDecorator.RunCondition.KeyExists,
                                    BlackboardDecorator.NotifyRule.RunConditionChange,
                                    BlackboardDecorator.NotifyAbort.both);

        root = checkHavingTargetDecorator;
    }
}
