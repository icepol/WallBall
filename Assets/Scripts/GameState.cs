using System.Collections;
using System.Collections.Generic;

public static class GameState
{
    public static bool IsFirstRun = true;
    public static int ActivePlayer = 0;
    public static bool IsGameRunning = false;
    public static int BallsCount = 11;

    public static Dictionary<string, int> Score = new Dictionary<string, int>();
}