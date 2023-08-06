using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardDecorator : Decorator
{
    public enum RunCondition
    {
        KeyExists,
        KeyNonExists
    }

    public enum NotifyRule
    {
        RunConditionChange,
        KeyValueChange
    }

    public enum NotifyAbort
    {
        none,
        self,
        lower,
        both
    }

    private string key;
    private object value;

    private RunCondition runCondition;
    private NotifyRule notifyRule;
    private NotifyAbort notifyAbort;

    public BlackboardDecorator(BehaviorTree tree, BTNode child, 
        string key, RunCondition runCondition, NotifyRule notifyRule, NotifyAbort notifyAbort) : base(tree, child)
    {
        this.key = key;
        this.runCondition = runCondition;
        this.notifyRule = notifyRule;
        this.notifyAbort = notifyAbort;
    }

    protected override NodeResult Execute()
    {
        Blackboard blackboard = behaviorTree.BlackBoard;
        if(blackboard == null)
        {
            return NodeResult.Failure;
        }
        blackboard.onBlackboardValueChange -= Blackboard_onBlackboardValueChange;
        blackboard.onBlackboardValueChange += Blackboard_onBlackboardValueChange;

        if (CheckRunCondition())
        {
            return NodeResult.InProgress;
        }
        else
        {
            return NodeResult.Failure;
        }
    }


    private void Blackboard_onBlackboardValueChange(string key, object value)
    {
        CheckNotify(key, value);
    }

    private void CheckNotify(string key, object value)
    {
        if (this.key != key) return;

        if(notifyRule == NotifyRule.RunConditionChange)
        {
            bool previousExist = this.value != null;
            bool currentExist = value != null;

            if(previousExist != currentExist)
            {
                Notify();
            }
        }
        else if(notifyRule == NotifyRule.KeyValueChange)
        {
            if(this.value != value)
            {
                Notify();
            }
        }
    }

    private void Notify()
    {
        switch (notifyAbort)
        {
            case NotifyAbort.none:
                break;

            case NotifyAbort.self:
                AbortSelf();
                break;

            case NotifyAbort.lower:
                AbortLower();
                break;

            case NotifyAbort.both:
                AbortBoth();
                break;
        }
    }

    private void AbortSelf()
    {
        Abort();
    }

    private void AbortLower()
    {
        behaviorTree.AbortLowerThan(Priority);
    }

    private void AbortBoth()
    {
        Abort();
        AbortLower();
    }

    private bool CheckRunCondition()
    {
        bool exist = behaviorTree.BlackBoard.GetBlackboardData(key, out value);

        switch (runCondition)
        {
            case RunCondition.KeyExists:
                return exist;

            case RunCondition.KeyNonExists:
                return !exist;

            default:
                return false;
        }
    }

    protected override NodeResult Update()
    {
        return Child.UpdateNode();
    }


    protected override void End()
    {
        base.End();

        Child.Abort();
    }
}
