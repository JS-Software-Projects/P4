using System;

namespace P4.HomeScreen;

 public class HomeScene : IScreen
    {
        private Button _playButton;
        private Button _quitButton;
        private SpriteFont _font;
        private Texture2D _buttonTexture;
        public event Action OnPlayClicked;
        public event Action OnQuitClicked;
        public HomeScene()
        {
            LoadContent();
            InitializeButtons();
        }

        private void LoadContent()
        {
            // Load font and button textures using the global ContentManager
            _font = Globals.spriteFont;
            _buttonTexture = new Texture2D(Globals.graphicsDevice, 1, 1);
            _buttonTexture.SetData(new[] { Color.White });
        }

        private void InitializeButtons()
        {
            // Define the button colors
            Color textColor = Color.White;
            Color backgroundColor = Color.Gray;
            Color hoverColor = Color.DarkGray;
            
            // Create the Play button
            _playButton = new Button(new Rectangle(Globals.WindowSize.X / 2 - 100, Globals.WindowSize.Y / 2 - 50, 200, 50), "Play", _font, _buttonTexture, textColor, backgroundColor, hoverColor);
            _playButton.Click += PlayButton_Click;

            // Create the Quit button
            _quitButton = new Button(new Rectangle(Globals.WindowSize.X / 2 - 100, Globals.WindowSize.Y / 2 + 10, 200, 50), "Quit", _font, _buttonTexture, textColor, backgroundColor, hoverColor);
            _quitButton.Click += QuitButton_Click;
        }

        private void PlayButton_Click()
        {
            // Code to start the game
            OnPlayClicked?.Invoke();
        }

        private void QuitButton_Click()
        {
            // Code to quit the game
            OnQuitClicked?.Invoke();
        }

        public void Update(GameTime gameTime, MouseState mouseState)
        {
            // Update logic for your HomeScreen
            MouseState currentMouseState = mouseState;
            _playButton.Update(currentMouseState);
            _quitButton.Update(currentMouseState);
        }

        public void Draw(GameTime gameTime)
        {
            Globals.SpriteBatch.Begin();
            _playButton.Draw(Globals.SpriteBatch);
            _quitButton.Draw(Globals.SpriteBatch);
            Globals.SpriteBatch.End();
        }
    }