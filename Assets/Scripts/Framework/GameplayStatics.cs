using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameplayStatics 
{
    public static void SetGamePaused(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
    }
}
