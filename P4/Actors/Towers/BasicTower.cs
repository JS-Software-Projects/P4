namespace P4.Actors.Towers;

public class BasicTower : Tower
{
    public BasicTower(Texture2D texture, Vector2 position, Color color) : base(texture, position, color)
    {
        Radius = 400;
        Damage = 10;
        Cost = 10;
        AttackSpeed = 1;
    }
    
}