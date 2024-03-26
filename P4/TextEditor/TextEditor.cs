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
        private Rectangle buttomLine;

        private Button playButton;

        // to move left and right up and down
        private KeyboardState previousKeyboardState;
        private MouseState previousMouseState;

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
            buttomLine = new Rectangle(textAreaX, textAreaY + textAreaHeight - 80, textAreaWidth, 80);
            

            // Initialize your button here
            Texture2D buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            buttonTexture.SetData(new[] { Color.White });
            int buttonX = textAreaX + textAreaWidth/2-90; // Centering of the text area
            playButton = new Button(new Rectangle(buttonX, graphicsDevice.Viewport.Height - 60, 180, 40), "Execute and run", spriteFont, buttonTexture, Color.Black, Color.Salmon, Color.DarkSalmon);
            playButton.Click += OnPlayButtonClick;

            //load file content
            // Get the current directory of the running program
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string outputDirectoryName = "../../../Output/output.txt"; // Name of your desired output directory
            string filePath = System.IO.Path.Combine(baseDirectory, outputDirectoryName);

            LoadFileContent(filePath);

            // to move left and right up and down
            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();

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
        public void LoadFileContent(string filePath)
        {
            // Check if the file exists before trying to read
            if (System.IO.File.Exists(filePath))
            {
                // Read all lines of the file
                string[] fileLines = System.IO.File.ReadAllLines(filePath);

                // Clear current text and reset cursor
                lines.Clear();
                currentLine = 0;
                cursorPosition = 0;

                // Add each line from the file to the lines list
                foreach (string line in fileLines)
                {
                    lines.Add(line);
                }

                // If the file is empty, ensure there is at least one empty line
                if (lines.Count == 0)
                {
                    lines.Add("");
                }
            }
            else
            {
                // If the file does not exist, log this or handle as needed
                System.Diagnostics.Debug.WriteLine($"File not found: {filePath}");
            }
        }

        public void AddCharacter(char character)
        {
            const int margin = 10; // Margin for the text area
            float availableWidth = textAreaRectangle.Width - (2 * margin); // Available width for text
            int maxHeight = Globals.WindowSize.Y - 90; // Maximum height for text lines

            // Calculate maximum number of lines allowed based on maxHeight and line spacing
            int maxLines = (maxHeight - textAreaRectangle.Y) / spriteFont.LineSpacing;

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
            else if (character == '\r' && lines.Count < maxLines)
            {
                lines.Insert(currentLine + 1, "");
                currentLine++;
                cursorPosition = 0;
            }
            // Handle normal characters
            else if (!char.IsControl(character))
            {
                // Prevent adding new characters if this is the last allowed line and it's already at max length
                if (currentLine == maxLines - 1 && spriteFont.MeasureString(lines[currentLine]).X >= availableWidth)
                {
                    return; // Do nothing more, prevent overflow
                }

                // Insert character into the current line at the current cursor position
                string newLine = lines[currentLine].Insert(cursorPosition, character.ToString());
                Vector2 newSize = spriteFont.MeasureString(newLine);

                // Check if the new line exceeds the available width
                if (newSize.X > availableWidth && lines.Count < maxLines)
                {
                    // Split the line at the last space (if exists) or at the current position
                    int lastSpace = newLine.LastIndexOf(' ', cursorPosition);
                    if (lastSpace > -1)
                    {
                        // Create a new line starting from the word after the last space
                        string nextLine = newLine.Substring(lastSpace + 1);
                        lines[currentLine] = newLine.Substring(0, lastSpace);
                        lines.Insert(currentLine + 1, nextLine);
                        cursorPosition = nextLine.Length;
                    }
                    else
                    {
                        // No space found, just split the line at the current position
                        lines[currentLine] = newLine.Substring(0, cursorPosition);
                        lines.Insert(currentLine + 1, newLine.Substring(cursorPosition));
                        cursorPosition = 1; // Start from the next character in the new line
                    }
                    currentLine++;
                }
                else
                {
                    // If the line does not exceed the available width, just update normally
                    lines[currentLine] = newLine;
                    cursorPosition++;
                }
            }
        }
        public void Update(MouseState mouseState)
        {
            // Update the button
            playButton.Update(mouseState);

            // Handle keyboard state
            KeyboardState keyboardState = Keyboard.GetState();
            HandleKeyboardInput(keyboardState);

            // Handle new mouse state
            HandleMouseInput(mouseState);
        }

        // Modify your HandleKeyboardInput method to use the previous state:
        private void HandleKeyboardInput(KeyboardState keyboardState)
        {
            // Handling the arrow keys with debouncing
            if (keyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left))
            {
                if (cursorPosition > 0)
                {
                    cursorPosition--;
                }
                else if (currentLine > 0)
                {
                    currentLine--;
                    cursorPosition = lines[currentLine].Length;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right))
            {
                if (cursorPosition < lines[currentLine].Length)
                {
                    cursorPosition++;
                }
                else if (currentLine < lines.Count - 1)
                {
                    currentLine++;
                    cursorPosition = 0;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
            {
                if (currentLine > 0)
                {
                    currentLine--;
                    cursorPosition = Math.Min(cursorPosition, lines[currentLine].Length);
                }
            }
            else if (keyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
            {
                if (currentLine < lines.Count - 1)
                {
                    currentLine++;
                    cursorPosition = Math.Min(cursorPosition, lines[currentLine].Length);
                }
            }

            // Update the previousKeyboardState at the end of the method
            previousKeyboardState = keyboardState;
        }
        private void HandleMouseInput(MouseState mouseState)
        {
            // Check for left mouse button click transition from not pressed to pressed
            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                // Calculate the clicked position relative to the text area
                int mouseX = mouseState.X;
                int mouseY = mouseState.Y;

                // Check if the click was inside the text area
                if (textAreaRectangle.Contains(mouseX, mouseY))
                {
                    int textStartX = textAreaRectangle.X + 10; // This should match your actual text margin
                    int clickX = mouseX - textStartX;
                    int clickY = mouseY - textAreaRectangle.Y;

                    // Determine which line was clicked
                    currentLine = Math.Min(clickY / spriteFont.LineSpacing, lines.Count - 1);
                    cursorPosition = 0;

                    // Find the nearest character position to the click
                    string currentTextLine = lines[currentLine];
                    float smallestDistance = float.MaxValue;

                    for (int i = 0; i <= currentTextLine.Length; i++)
                    {
                        string textUpToCursor = currentTextLine.Substring(0, i);
                        Vector2 textSize = spriteFont.MeasureString(textUpToCursor);
                        float distance = Math.Abs(clickX - textSize.X);

                        if (distance < smallestDistance)
                        {
                            smallestDistance = distance;
                            cursorPosition = i;
                        }
                    }
                }
            }
            previousMouseState = mouseState;
        }
        public void Draw()
        {
            spriteBatch.Begin();
            Texture2D textAreaBackground = new Texture2D(graphicsDevice, 1, 1);
            textAreaBackground.SetData(new Color[] { Color.LightGray });
            spriteBatch.Draw(textAreaBackground, textAreaRectangle, Color.White); // Use the calculated rectangle
            spriteBatch.Draw(textAreaBackground, numberArea, Color.LightSeaGreen);
            spriteBatch.Draw(textAreaBackground, buttomLine, Color.LightSeaGreen);
            DrawBorder(spriteBatch, textAreaRectangle, 2, Color.Black);
            DrawBorder(spriteBatch, numberArea, 2, Color.Black);
            DrawBorder(spriteBatch, buttomLine, 2, Color.Black);

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
        private void DrawBorder(SpriteBatch spriteBatch, Rectangle rectangle, int thickness, Color color)
        {
            Texture2D pixelTexture; // Declare this at the class level

            // In your initialization method:
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White }); // Fill the texture with white color

            // Draw top line
            spriteBatch.Draw(texture: pixelTexture, destinationRectangle: new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color: color);

            // Draw left line
            spriteBatch.Draw(texture: pixelTexture, destinationRectangle: new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color: color);

            // Draw right line
            spriteBatch.Draw(texture: pixelTexture, destinationRectangle: new Rectangle(rectangle.Right, rectangle.Y, thickness, rectangle.Height), color: color);

            // Draw bottom line
            spriteBatch.Draw(texture: pixelTexture, destinationRectangle: new Rectangle(rectangle.X, rectangle.Bottom, rectangle.Width, thickness), color: color);
        }

    }
}
