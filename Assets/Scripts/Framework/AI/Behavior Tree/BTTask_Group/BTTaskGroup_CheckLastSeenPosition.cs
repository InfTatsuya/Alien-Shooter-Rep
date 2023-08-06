using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTaskGroup_CheckLastSeenPosition : BTTask_Group
{
    private float acceptableDistance = 1.5f;

    public BTTaskGroup_CheckLastSeenPosition(BehaviorTree behaviourTree, float acceptableDistance) : base(behaviourTree)
    {
        this.acceptableDistance = acceptableDistance;
    }

    protected override void ConstructTree(out BTNode root)
    {
        Sequencer checkLastSeenSequencer = new Sequencer();
        BTTask_MoveToLocation moveToLastSeenPos =
            new BTTask_MoveToLocation(behaviorTree, StringCollector.lastSeenPosString, acceptableDistance);
        BTTask_Wait waitAtLastSeenPos = new BTTask_Wait(3f);
        BTTask_RemoveBlackboardData removeLastSeenPos =
            new BTTask_RemoveBlackboardData(behaviorTree, StringCollector.lastSeenPosString);

        checkLastSeenSequencer.AddChild(moveToLastSeenPos);
        checkLastSeenSequencer.AddChild(waitAtLastSeenPos);
        checkLastSeenSequencer.AddChild(removeLastSeenPos);

        BlackboardDecorator checkHavingLastSeenPosDecorator =
            new BlackboardDecorator(behaviorTree,
                                    checkLastSeenSequencer,
                                    StringCollector.lastSeenPosString,
                                    BlackboardDecorator.RunCondition.KeyExists,
                                    BlackboardDecorator.NotifyRule.RunConditionChange,
                                    BlackboardDecorator.NotifyAbort.none);

        root = checkHavingLastSeenPosDecorator;
        
    }
}
