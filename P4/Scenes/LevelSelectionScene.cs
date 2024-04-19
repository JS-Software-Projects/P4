using System;
using System.IO;

namespace P4.HomeScreen;
    public class LevelSelectionScene : IScreen
    {
        private Button[] _levelButtons;
        private SpriteFont _font;
        private Texture2D _buttonTexture;
        public event Action<string> LevelSelected;
        public LevelSelectionScene()
        {
            LoadContent();
            InitializeLevelButtons();
        }

        private void LoadContent()
        {
            // Assuming these are loaded from the Globals class
            _font = Globals.spriteFont;
            _buttonTexture = new Texture2D(Globals.graphicsDevice, 1, 1);
            _buttonTexture.SetData(new[] { Color.White });
        }

        private void InitializeLevelButtons()
        {
            string levelsDirectory = Path.Combine(Globals.Content.RootDirectory, "../../../../Levels");
            string[] levelFiles = Directory.GetFiles(levelsDirectory, "*.txt");
            _levelButtons = new Button[levelFiles.Length];

            int yPos = 100;
            for (int i = 0; i < levelFiles.Length; i++)
            {
                string levelPath = levelFiles[i];
                string levelName = Path.GetFileNameWithoutExtension(levelPath);
                Rectangle buttonBounds = new Rectangle(Globals.WindowSize.X / 2 - 100, yPos, 200, 50);
                _levelButtons[i] = new Button(buttonBounds, levelName, _font, _buttonTexture, Color.White, Color.Gray, Color.DarkGray);
                _levelButtons[i].Click += () => LevelSelected?.Invoke(levelPath); // Pass the full path

                yPos += 60; // Increment y position for the next button
            }
        }

        public void Update(GameTime gameTime,MouseState mouseState)
        {
            MouseState currentMouseState = mouseState;
            foreach (var button in _levelButtons)
            {
                button.Update(currentMouseState);
            }
        }

        public void Draw(GameTime gameTime)
        {
            Globals.SpriteBatch.Begin();
            foreach (var button in _levelButtons)
            {
                button.Draw(Globals.SpriteBatch);
            }
            Globals.SpriteBatch.End();
        }
    }

