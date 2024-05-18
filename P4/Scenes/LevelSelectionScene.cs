using System;
using System.Collections.Generic;
using System.IO;

namespace P4.HomeScreen;
    public class LevelSelectionScene : IScreen
    {
        private Button[] _levelButtons;
        private SpriteFont _font;
        private Texture2D _buttonTexture;
        public event Action<string> LevelSelected;
        private HashSet<string> _completedLevels;
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
            LoadCompletedLevels();
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
        private HashSet<string> LoadCompletedLevels()
        {
            HashSet<string> completedLevels = new HashSet<string>();
            string filePath = Path.Combine(Globals.Content.RootDirectory, "completedLevels.txt");

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    completedLevels.Add(line.Trim());
                }
            }
            else
            {
                File.Create(filePath).Close();
            }

            return completedLevels;
        }

        public void MarkLevelAsCompleted(string levelName)
        {
            if (!_completedLevels.Contains(levelName))
            {
                _completedLevels.Add(levelName);
                SaveCompletedLevels();
            }
        }

        private void SaveCompletedLevels()
        {
            string filePath = Path.Combine(Globals.Content.RootDirectory, "completedLevels.txt");
            File.WriteAllLines(filePath, _completedLevels);
        }
        private bool IsLevelCompleted(string levelName)
        {
            if (_completedLevels == null)
            {
                _completedLevels = LoadCompletedLevels();
            }
            return _completedLevels.Contains(levelName);
        }
        
        public void Update(GameTime gameTime,MouseState mouseState)
        {
            MouseState currentMouseState = mouseState;
            foreach (var button in _levelButtons)
            {
                button.Update(currentMouseState);
                // Update button color based on level completion status
                if (IsLevelCompleted(button.Text))
                {
                    button.setColor(Color.Green);
                    button.setHover(Color.LimeGreen);
                }
                else
                {
                    button.setColor(Color.Gray);
                }
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

