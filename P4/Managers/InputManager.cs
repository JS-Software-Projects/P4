using System;

namespace P4.managers;

public static class InputManager
{
    public static bool execute { get; private set; }
    public static Rectangle tile { get; private set; }
    
    public static void SetExecute(bool run,int x, int y)
    {
        execute = run;
        tile = new(x*Globals.TileSize-(Globals.TileSize/2), y*Globals.TileSize-(Globals.TileSize/2), 1, 1);
    }
}
