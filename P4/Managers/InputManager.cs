using System;

namespace P4.managers;

public static class InputManager
{
    public static bool execute { get; private set; }
    public static Rectangle tile { get; private set; }
    
    public static void SetExecute(bool run,int x, int y)
    {
        execute = run;
        tile = new(x*63, y*63, 1, 1);
    }
}
