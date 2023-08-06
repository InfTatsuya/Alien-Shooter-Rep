using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SenseComponent : MonoBehaviour
{
    private static List<PerceptionStimulus> registeredStimuluses = new List<PerceptionStimulus>();

    public static void RegisterStimulus(PerceptionStimulus stimulus)
    {
        if(registeredStimuluses.Contains(stimulus)) return;

        registeredStimuluses.Add(stimulus);
    }

    public static void UnregisterStimulus(PerceptionStimulus stimulus)
    {
        registeredStimuluses.Remove(stimulus);   
    }

    public delegate void OnPerceptionUpdated(PerceptionStimulus stimulus, bool successfullySensed);
    public event OnPerceptionUpdated onPerceptionUpdated;

    [SerializeField] float forgettingTime = 5f;

    private List<PerceptionStimulus> perceivableStimuluses = new List<PerceptionStimulus>();
    private Dictionary<PerceptionStimulus, Coroutine> forgettingRoutines = new Dictionary<PerceptionStimulus, Coroutine>();


    protected abstract bool InStimuliSensable(PerceptionStimulus stimulus);

    private void Update()
    {
        foreach(var stimulus in registeredStimuluses)
        {
            if (InStimuliSensable(stimulus))
            {
                if (!perceivableStimuluses.Contains(stimulus))
                {
                    perceivableStimuluses.Add(stimulus);

                    if(forgettingRoutines.TryGetValue(stimulus, out Coroutine coroutine))
                    {
                        StopCoroutine(coroutine);
                        forgettingRoutines.Remove(stimulus);
                    }
                    else
                    {
                        onPerceptionUpdated?.Invoke(stimulus, true);
                        Debug.Log($"Sensed {stimulus.gameObject.name}");
                    }

                }
            }
            else
            {
                if (perceivableStimuluses.Contains(stimulus))
                {
                    perceivableStimuluses.Remove(stimulus);

                    forgettingRoutines.Add(stimulus ,StartCoroutine(ForgettingStimulus(stimulus)));
                }
            }
        }
    }

    private IEnumerator ForgettingStimulus(PerceptionStimulus stimulus)
    {
        yield return new WaitForSeconds(forgettingTime);

        forgettingRoutines.Remove(stimulus);
        onPerceptionUpdated?.Invoke(stimulus, false);
        Debug.Log($"Lost track of {stimulus.gameObject.name}");
    }

    protected virtual void OnDrawGizmos()
    {
        
    }

    internal void AssignPerceiveStimulus(PerceptionStimulus stimulus)
    {
        perceivableStimuluses.Add(stimulus);
        onPerceptionUpdated?.Invoke(stimulus, true);

        if(forgettingRoutines.TryGetValue(stimulus, out Coroutine forgetRoutine))
        {
            StopCoroutine(forgetRoutine);
            forgettingRoutines.Remove(stimulus);
        }
    }
}
