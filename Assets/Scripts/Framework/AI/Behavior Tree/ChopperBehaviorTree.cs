using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopperBehaviorTree : BehaviorTree
{
    [SerializeField] float acceptableDistance = 1.5f;
    [SerializeField] float acceptableDegrees = 10f;
    

    protected override void ConstructTree(out BTNode rootNode)
    {
        //Sequencer attackSequencer = new Sequencer();

        //BTTask_MoveToTarget moveToTarget = 
        //    new BTTask_MoveToTarget(this, StringCollector.targetString, acceptableDistance);
        //BTTask_RotateTowardTarget rotateTowardTarget =
        //    new BTTask_RotateTowardTarget(this, StringCollector.targetString, acceptableDegrees);
        //BTTask_AttackTarget attackTarget =
        //    new BTTask_AttackTarget(this, StringCollector.targetString);

        ////BTTask_Wait waitToRepeatAttack = new BTTask_Wait(1f);

        //attackSequencer.AddChild(moveToTarget);
        //attackSequencer.AddChild(rotateTowardTarget);
        //attackSequencer.AddChild(attackTarget);

        //BlackboardDecorator checkHavingTargetDecorator =
        //    new BlackboardDecorator(this,
        //                            attackSequencer,
        //                            StringCollector.targetString,
        //                            BlackboardDecorator.RunCondition.KeyExists,
        //                            BlackboardDecorator.NotifyRule.RunConditionChange,
        //                            BlackboardDecorator.NotifyAbort.both);

        BTTaskGroup_AttackTarget attackTarget = new BTTaskGroup_AttackTarget(this, acceptableDistance, acceptableDegrees, 0f);

        //Sequencer checkLastSeenSequencer = new Sequencer();
        //BTTask_MoveToLocation moveToLastSeenPos =
        //    new BTTask_MoveToLocation(this, StringCollector.lastSeenPosString, acceptableDistance);
        //BTTask_Wait waitAtLastSeenPos = new BTTask_Wait(3f);
        //BTTask_RemoveBlackboardData removeLastSeenPos =
        //    new BTTask_RemoveBlackboardData(this, StringCollector.lastSeenPosString);

        //BlackboardDecorator checkHavingLastSeenPosDecorator =
        //    new BlackboardDecorator(this,
        //                            checkLastSeenSequencer,
        //                            StringCollector.lastSeenPosString,
        //                            BlackboardDecorator.RunCondition.KeyExists,
        //                            BlackboardDecorator.NotifyRule.RunConditionChange,
        //                            BlackboardDecorator.NotifyAbort.none);


        //checkLastSeenSequencer.AddChild(moveToLastSeenPos);
        //checkLastSeenSequencer.AddChild(waitAtLastSeenPos);
        //checkLastSeenSequencer.AddChild(removeLastSeenPos);

        BTTaskGroup_CheckLastSeenPosition checkLastSeenPos =
            new BTTaskGroup_CheckLastSeenPosition(this, acceptableDistance);

        //Sequencer patrollingSequence = new Sequencer();
        //BTTask_GetNextPatrolPoint getNextPatrolPoint = new BTTask_GetNextPatrolPoint(this);
        //BTTask_MoveToLocation moveTo = new BTTask_MoveToLocation(this, StringCollector.patrolPointString, acceptableDistance);
        //BTTask_Wait waitAtPatrolPoint = new BTTask_Wait(2f);

        //patrollingSequence.AddChild(getNextPatrolPoint);
        //patrollingSequence.AddChild(moveTo);
        //patrollingSequence.AddChild(waitAtPatrolPoint);

        BTTaskGroup_Patrolling patrolling =
            new BTTaskGroup_Patrolling(this, acceptableDistance);

        Selector rootSelector = new Selector();

        rootSelector.AddChild(attackTarget);
        rootSelector.AddChild(checkLastSeenPos);
        rootSelector.AddChild(patrolling);

        rootNode = rootSelector;

    }
}
