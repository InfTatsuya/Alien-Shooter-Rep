using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionComponent : MonoBehaviour
{
    public delegate void OnPerceptionTargetChanged(GameObject target, bool sensed);
    public event OnPerceptionTargetChanged onPerceptionTargetChanged;


    [SerializeField] SenseComponent[] senses;

    private LinkedList<PerceptionStimulus> currentlyPerceivedStimuluses = new LinkedList<PerceptionStimulus>();

    private PerceptionStimulus targetStimulus;

    private void Awake()
    {
        foreach(var sense in senses)
        {
            sense.onPerceptionUpdated += SenseUpdated;
        }
    }

    private void SenseUpdated(PerceptionStimulus stimulus, bool successfullySensed)
    {
        LinkedListNode<PerceptionStimulus> nodeFound = currentlyPerceivedStimuluses.Find(stimulus);
        
        if (successfullySensed)
        {
            if(nodeFound != null )
            {
                currentlyPerceivedStimuluses.AddAfter(nodeFound, stimulus);
            }
            else
            {
                currentlyPerceivedStimuluses.AddLast(stimulus);
            }
        }
        else
        {
            currentlyPerceivedStimuluses.Remove(nodeFound);
        }

        if(currentlyPerceivedStimuluses.Count > 0)
        {
            PerceptionStimulus highestStimulus = currentlyPerceivedStimuluses.First.Value;
        
            if(targetStimulus == null || targetStimulus != highestStimulus)
            {
                targetStimulus = highestStimulus;
                onPerceptionTargetChanged?.Invoke(targetStimulus.gameObject, true);
            }
        }
        else if(targetStimulus != null)
        {
            onPerceptionTargetChanged?.Invoke(targetStimulus.gameObject, false);
            targetStimulus = null;
        }
    }

    internal void AssignPerceiveStimulus(PerceptionStimulus stimulus)
    {
        if(senses.Length > 0)
        {
            senses[0].AssignPerceiveStimulus(stimulus);
        }
    }
}
