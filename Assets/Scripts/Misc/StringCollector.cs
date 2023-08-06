using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringCollector 
{
    public const string targetString = "Target";
    public const string patrolPointString = "PatrolPoint";
    public const string lastSeenPosString = "LastSeenPosition";

    public static int deadAnim = Animator.StringToHash("dead");
    public static int attackAnim = Animator.StringToHash("attack");
    public static int speedAnim = Animator.StringToHash("speed");
    public static int spawnAnim = Animator.StringToHash("spawn");
}
