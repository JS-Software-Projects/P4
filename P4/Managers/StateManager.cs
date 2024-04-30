using System;

namespace P4;

public class StateManager
{
    private IScreen _currentScreen;
    public event Action<IScreen> ScreenChanged;
    public void ChangeScreen(IScreen newScreen)
    {
        _currentScreen = newScreen;
        ScreenChanged?.Invoke(newScreen);
    }

    public void Update(GameTime gameTime, MouseState mouseState)
    {
        _currentScreen?.Update(gameTime, mouseState);
    }

    public void Draw(GameTime gameTime)
    {
        _currentScreen?.Draw(gameTime);
    }
}
