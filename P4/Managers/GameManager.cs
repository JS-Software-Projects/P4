using p4.Actors;
using P4.managers;

namespace P4.Managers;

public class GameManager
{
    private readonly Map _map;
    private readonly Hero _hero;

    public GameManager()
    {
        _map = new();
        var position = _map.MapToScreen(0, 3);
        _hero = new(Globals.Content.Load<Texture2D>("hero"),position);
        Pathfinder.Init(_map, _hero);
    }

    public void Update()
    {
        _map.Update();
        _hero.Update();
    }

    public void Draw()
    {
        Globals.SpriteBatch.Begin();
        _map.Draw();
        _hero.Draw();
        Globals.SpriteBatch.End();
    }
}