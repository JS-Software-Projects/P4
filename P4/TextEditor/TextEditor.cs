using System;
using System.Collections.Generic;

namespace P4
{
    public class TextEditor
    {
        private List<string> lines = new List<string> { "" };
        private int currentLine = 0;
        private int cursorPosition = 0;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private SpriteFont spriteFont;
        private Rectangle textAreaRectangle;
        private Rectangle numberArea;

        private Button playButton;
        
        public TextEditor(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;

            // Calculate dimensions to fill 20% of the right-hand side of the window
            int textAreaWidth = (int)(Globals.WindowSize.X * 0.35); // 20% of the window width, accessed directly from Globals
            int textAreaHeight = Globals.WindowSize.Y; // Full height, accessed directly from Globals
            int textAreaX = Globals.WindowSize.X - textAreaWidth; // Positioned on the right, accessed directly from Globals
            int textAreaY = 0; // Start at the top

            textAreaRectangle = new Rectangle(textAreaX, textAreaY, textAreaWidth, textAreaHeight);
            numberArea = new Rectangle(textAreaX - 40, textAreaY, 40, textAreaHeight);

            
            // Initialize your button here
            Texture2D buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            buttonTexture.SetData(new[] { Color.White });
            int buttonX = textAreaX + textAreaWidth/2-90; // Centering of the text area
            playButton = new Button(new Rectangle(buttonX, graphicsDevice.Viewport.Height - 60, 180, 40), "Execute and run", spriteFont, buttonTexture, Color.Black, Color.Salmon, Color.DarkSalmon);
            playButton.Click += OnPlayButtonClick;
        }
        private void OnPlayButtonClick()
        {
            // Actions to take when the button is clicked
          
            
            // Concatenate all lines into a single string
            string allText = string.Join("\n", lines);

            // Get the current directory of the running program
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string outputDirectoryName = "../../../Output"; // Name of your desired output directory
            string directoryPath = System.IO.Path.Combine(baseDirectory, outputDirectoryName);

            // Create the output directory if it does not exist
            System.IO.Directory.CreateDirectory(directoryPath);

            // Combine the directory path with the desired file name and write the text to the file
            string fullPath = System.IO.Path.Combine(directoryPath, "output.txt");
            System.IO.File.WriteAllText(fullPath, allText);
         
            // Log the full path or open the folder containing the file
            System.Diagnostics.Debug.WriteLine($"File written to: {fullPath}");
            

        }
        public void AddCharacter(char character)
        {
            // Handle backspace
            if (character == '\b')
            {
                if (cursorPosition > 0)
                {
                    lines[currentLine] = lines[currentLine].Remove(cursorPosition - 1, 1);
                    cursorPosition--;
                }
                else if (currentLine > 0)
                {
                    cursorPosition = lines[currentLine - 1].Length;
                    lines[currentLine - 1] += lines[currentLine];
                    lines.RemoveAt(currentLine);
                    currentLine--;
                }
            }
            // Handle new line
            else if (character == '\r')
            {
                lines.Insert(currentLine + 1, "");
                currentLine++;
                cursorPosition = 0;
            }
            // Handle normal characters
            else if (!char.IsControl(character))
            {
                lines[currentLine] = lines[currentLine].Insert(cursorPosition, character.ToString());
                cursorPosition++;
            }
        }
        public void Update(MouseState mouseState)
        {
            // Update the button
            playButton.Update(mouseState);
        }

        public void Draw()
        {
            spriteBatch.Begin();
            Texture2D textAreaBackground = new Texture2D(graphicsDevice, 1, 1);
            textAreaBackground.SetData(new Color[] { Color.LightGray });
            spriteBatch.Draw(textAreaBackground, textAreaRectangle, Color.White); // Use the calculated rectangle
            spriteBatch.Draw(textAreaBackground, numberArea, Color.LightSeaGreen);

            // Set starting position for text (adjust margins as needed)
            int textStartX = textAreaRectangle.X + 10; // 10 pixels from the left edge of the text area
            int textStartY = textAreaRectangle.Y + 10; // 10 pixels from the top edge of the text area

            for (int i = 0; i < lines.Count; i++)
            {
                string lineNumber = (i + 1).ToString() + "."; // Line numbers start at 1
                string line = lines[i];
                spriteBatch.DrawString(spriteFont, line, new Vector2(textStartX, textStartY + i * spriteFont.LineSpacing), Color.Black);
                
                // Draw line number
                spriteBatch.DrawString(spriteFont, lineNumber, new Vector2(textStartX-40, textStartY + i * spriteFont.LineSpacing), Color.Black);
              
                // Draw cursor for the current line
                if (i == currentLine)
                {
                    string textBeforeCursor = line.Substring(0, cursorPosition);
                    Vector2 cursorPositionMeasure = spriteFont.MeasureString(textBeforeCursor);
                    spriteBatch.DrawString(spriteFont, "|", new Vector2(textStartX + cursorPositionMeasure.X, textStartY + i * spriteFont.LineSpacing), Color.Black);
                }
            }
            playButton.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
