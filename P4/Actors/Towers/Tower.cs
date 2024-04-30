using System;
using System.Collections.Generic;
using p4.Actors;

namespace P4.Actors.Towers;

public abstract class Tower : Sprite
{

    private Vector2? targetPosition;  // Optional position of a target; nullable if no target is present
    public float Rotation { get; private set; }  // Current rotation of the tower in radians

    public Tower(Texture2D texture, Vector2 position, Color color) : base(texture, position)
    {
        this.Position = position;
        this.Color = color;
        Origin = Vector2.Zero;
    }

    public int Radius { get; set; }
    public int Damage { get; set; }
    public int Cost { get; set; }
    public int AttackSpeed { get; set; }
    
    public void UpdateTarget(Vector2? newPosition)
    {
        targetPosition = newPosition;
        if (targetPosition.HasValue)
        {
            // Calculate the angle to the target
            Vector2 direction = targetPosition.Value - Position;
            float targetAngle = (float)Math.Atan2(direction.Y, direction.X);

            // Since the texture is facing upwards, adjust the angle by subtracting π/2
            float baseRotation = +(float)Math.PI / 2;

            // Set the rotation, adding the base rotation
            Rotation = targetAngle + baseRotation;
        }
    }

    // Inside your Tower class
    public void Update(List<Vector2> enemyPositions)
    {
        Vector2? closestEnemy = null;
        float closestDistanceSquared = Radius * Radius;  // Use squared distance to avoid unnecessary square root calculations

        foreach (var enemyPos in enemyPositions)
        {
            float distanceSquared = Vector2.DistanceSquared(Position, enemyPos);
            if (distanceSquared < closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestEnemy = enemyPos;
            }
        }

        UpdateTarget(closestEnemy);
    }


    public override void Draw()
    {
        // Calculate the scale to make the tower slightly smaller than the tile
        float scale = Math.Min((Globals.TileSize - 10) / (float)texture.Width, (Globals.TileSize - 10) / (float)texture.Height);

        // Calculate the size of the texture after scaling
        float scaledWidth = texture.Width * scale;
        float scaledHeight = texture.Height * scale;

        // Calculate how much empty space remains within the tile after scaling the texture
        float remainingWidth = Globals.TileSize - scaledWidth;
        float remainingHeight = Globals.TileSize - scaledHeight;
        Origin = new Vector2(texture.Width / 2, texture.Height / 2);

        // Adjust the position to center the scaled texture within the same tile
        Vector2 newPosition = new Vector2(Position.X - Globals.TileSize/2, Position.Y - Globals.TileSize/2);

        // Draw the tower with the specified scale
        Globals.SpriteBatch.Draw(texture, newPosition, null, Color, Rotation, Origin, scale, SpriteEffects.None, 0f);
    }

}