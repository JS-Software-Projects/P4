namespace P4.Actors.Towers;

internal class BasicTower : Tower
{
    public BasicTower(Texture2D texture, Vector2 position, Color color) : base(texture, position, color)
    {
        radius = 100;
        damage = 10;
        cost = 10;
        attackSpeed = 1;
    }
}