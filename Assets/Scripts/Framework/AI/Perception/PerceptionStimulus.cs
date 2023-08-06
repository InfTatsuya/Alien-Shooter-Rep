using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionStimulus : MonoBehaviour
{
    void Start()
    {
        SenseComponent.RegisterStimulus(this);
    }

    private void OnDestroy()
    {
        SenseComponent.UnregisterStimulus(this);
    }
}
