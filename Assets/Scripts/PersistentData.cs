using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentData {
    public static float lives = 4;
    public static bool isModified = false;
    public static bool didWin;
    public static bool hardMode = false;
    public static float currlevels = 0;

    // Stores a shuffled order for the first four minigames so they don't repeat
    // Huntsman, LosPollosHermanos, SaulSwitchNumbers, Yummers (randomized once per run)
    public static string[] minigameOrder = null;
    public static bool minigameOrderInitialized = false;
}
