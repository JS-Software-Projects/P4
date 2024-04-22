namespace P4;

public interface IScreen
{
    void Update(GameTime gameTime, MouseState mouseState);
    void Draw(GameTime gameTime);
}