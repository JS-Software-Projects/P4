using System;
using Microsoft.Xna.Framework.Content;
using P4.HomeScreen;
using P4.managers;

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
        _homeIconTexture = Globals.Content.Load<Texture2D>("HomeIcon3");
        _homeIconBounds = new Rectangle(5, 5, 35, 35); // Example bounds
    }

    public void Update()
    {
        MouseState currentMouseState = Mouse.GetState();
        _isHovering = _homeIconBounds.Contains(currentMouseState.X, currentMouseState.Y);

        if (_isHovering && currentMouseState.LeftButton == ButtonState.Released &&
            _previousMouseState.LeftButton == ButtonState.Pressed)
        {
            HomeClicked?.Invoke(); // Raise the event when the home icon is clicked
            InputManager.SetExecute(false,8,8);
        }

        _previousMouseState = currentMouseState; // Update the previous mouse state
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Color color = _isHovering ? new Color(255, 255, 255, 150) : Color.White; // Adjust alpha or blend color
        spriteBatch.Draw(_homeIconTexture, _homeIconBounds, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
    }
}