using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BestScores
{
    const string KEY_FALLING_CUBES = "FallingCubesBest";
    const string KEY_FAST_TYPING = "FastTypingBest";
    
    public static int FallingCubesBest { 
        get { return PlayerPrefs.GetInt(KEY_FALLING_CUBES, 0); }
        set { PlayerPrefs.SetInt(KEY_FALLING_CUBES, value); }
    }

    public static int FastTypingBest { 
        get { return PlayerPrefs.GetInt(KEY_FAST_TYPING, 0); }
        set { PlayerPrefs.SetInt(KEY_FAST_TYPING, value); }
    }
}
