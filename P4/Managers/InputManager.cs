using System.Collections.Generic;
using Microsoft.Xna.Framework;

public static class InputManager
{
    public static bool execute { get; private set; }
    private static Queue<Rectangle> targetLocations = new Queue<Rectangle>();

    public static Rectangle CurrentTarget
    {
        get
        {
            if (targetLocations.Count > 0)
            {
                return targetLocations.Peek();
            }
            return Rectangle.Empty;
        }
    }

    public static void SetExecute(bool run, int x, int y)
    {
        execute = run;
        if (execute)
        {
            var target = new Rectangle(x * Globals.TileSize - (Globals.TileSize / 2), y * Globals.TileSize - (Globals.TileSize / 2), 1, 1);
            targetLocations.Enqueue(target);
        }
    }

    public static void MoveToNextTarget()
    {
        if (targetLocations.Count > 0)
        {
            targetLocations.Dequeue();
            if (targetLocations.Count == 0)
            {
                execute = false;
            }
        }
    }
}