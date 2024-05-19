using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace P4.HomeScreen
{
    public class LevelSelectionScene : IScreen
    {
        private Button[] _levelButtons;
        private SpriteFont _font;
        private Texture2D _buttonTexture;
        private MessageTextBox _introBox;
        public event Action<string> LevelSelected;
        private HashSet<string> _completedLevels;
        private static readonly object fileLock = new object();
        private const string CompletedLevelsFileName = "completedLevels.txt";

        public LevelSelectionScene()
        {
            _completedLevels = LoadCompletedLevels();
            LoadContent();
            InitializeLevelButtons();
            _introBox = new MessageTextBox(new Rectangle(320, 20, 370, 0), " Welcome to the level selection screen!\n              Select a level to play!");
            
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

                // Set button color based on completion status
                Color buttonColor = IsLevelCompleted(levelName) ? Color.Green : Color.Gray;

                _levelButtons[i] = new Button(buttonBounds, levelName, _font, _buttonTexture, Color.White, buttonColor, Color.DarkGray);
                _levelButtons[i].Click += () => LevelSelected?.Invoke(levelPath); // Pass the full path

                yPos += 60; // Increment y position for the next button
            }
        }

        private HashSet<string> LoadCompletedLevels()
        {
            HashSet<string> completedLevels = new HashSet<string>();
            string filePath = Path.Combine(Globals.Content.RootDirectory, CompletedLevelsFileName);

            lock (fileLock)
            {
                if (File.Exists(filePath))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            completedLevels.Add(line.Trim());
                        }
                    }
                }
                else
                {
                    File.Create(filePath).Close();
                }
            }

            return completedLevels;
        }

        private void SaveCompletedLevels()
        {
            string filePath = Path.Combine(Globals.Content.RootDirectory, CompletedLevelsFileName);

            lock (fileLock)
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (var level in _completedLevels)
                    {
                        sw.WriteLine(level);
                    }
                }
            }
        }

        public void ResetCompletedLevels()
        {
            string filePath = Path.Combine(Globals.Content.RootDirectory, CompletedLevelsFileName);

            lock (fileLock)
            {
                File.WriteAllText(filePath, string.Empty);
                _completedLevels.Clear();
            }
        }

        public void MarkLevelAsCompleted(string levelName)
        {
            if (!_completedLevels.Contains(levelName))
            {
                _completedLevels.Add(levelName);
                SaveCompletedLevels();
            }
        }

        private bool IsLevelCompleted(string levelName)
        {
            return _completedLevels.Contains(levelName);
        }

        public void Update(GameTime gameTime, MouseState mouseState)
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
            _introBox.Draw(Globals.SpriteBatch);
            foreach (var button in _levelButtons)
            {
                button.Draw(Globals.SpriteBatch);
            }
            Globals.SpriteBatch.End();
        }
    }
}
