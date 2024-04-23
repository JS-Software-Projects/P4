using System;
using Microsoft.Xna.Framework.Content;
using P4.HomeScreen;

namespace P4;

public class UIManager
{
    private Texture2D _homeIconTexture;
    private Rectangle _homeIconBounds;
    private bool _isHovering;
    private MouseState _previousMouseState;

    public event Action HomeClicked;

    public UIManager( )
    {
        _homeIconTexture = Globals.Content.Load<Texture2D>("HomeIcon");
        _homeIconBounds = new Rectangle(10, 10, 50, 50); // Example bounds
    }

    public void Update()
    {
        MouseState currentMouseState = Mouse.GetState();
        _isHovering = _homeIconBounds.Contains(currentMouseState.X, currentMouseState.Y);

        if (_isHovering && currentMouseState.LeftButton == ButtonState.Released &&
            _previousMouseState.LeftButton == ButtonState.Pressed)
        {
            HomeClicked?.Invoke(); // Raise the event when the home icon is clicked
        }

        _previousMouseState = currentMouseState; // Update the previous mouse state
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Color color = _isHovering ? Color.LightGray : Color.White; // Change color on hover
        spriteBatch.Draw(_homeIconTexture, _homeIconBounds, color);
    }
}