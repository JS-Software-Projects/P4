using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using P4.Interpreter;
using P4.managers;

namespace P4;

public class TextEditor
{
    private readonly Rectangle buttomLine;
    private int currentLine;
    private int cursorPosition;
    private readonly GraphicsDevice graphicsDevice;
    private readonly List<string> lines = new() { "" };
    private readonly Rectangle numberArea;
    string _localfilePath = "";
    public event EventHandler ResetRequested;
    private readonly Button playButton;

    // to move left and right up and down
    private KeyboardState previousKeyboardState;
    private MouseState previousMouseState;
    private readonly SpriteBatch spriteBatch;
    private readonly SpriteFont spriteFont;
    private Rectangle textAreaRectangle;
    private MessageTextBox _textBox;

    public TextEditor(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, SpriteFont spriteFont,string localfilePath)
    {
        this.graphicsDevice = graphicsDevice;
        this.spriteBatch = spriteBatch;
        this.spriteFont = spriteFont;
        _localfilePath = localfilePath;
        
        // Calculate dimensions to fill 35% of the right-hand side of the window
        var textAreaWidth =
            (int)(Globals.WindowSize.X * 0.38)+4; // 35% of the window width, accessed directly from Globals
        var textAreaHeight = Globals.WindowSize.Y; // Full height, accessed directly from Globals
        var textAreaX = Globals.WindowSize.X - textAreaWidth; // Positioned on the right, accessed directly from Globals
        var textAreaY = 0; // Start at the top

        textAreaRectangle = new Rectangle(textAreaX, textAreaY, textAreaWidth, textAreaHeight);
        numberArea = new Rectangle(textAreaX - 40, textAreaY, 40, textAreaHeight);
        buttomLine = new Rectangle(textAreaX, textAreaY + textAreaHeight - 80, textAreaWidth, 80);
        
        var textboxRectangle = new Rectangle(50, 50, 500, 500);
        _textBox = new MessageTextBox(new Rectangle(620, 472, 370, 0), "For this level, try to move the hero\nto the exit using");
        
        
        // Initialize your button here
        var buttonTexture = new Texture2D(graphicsDevice, 1, 1);
        buttonTexture.SetData(new[] { Color.White });
        var buttonX = textAreaX + textAreaWidth / 2 - 90; // Centering of the text area
        playButton = new Button(new Rectangle(buttonX, graphicsDevice.Viewport.Height - 60, 180, 40), "Execute and run",
            spriteFont, buttonTexture, Color.Black, Color.Salmon, Color.DarkSalmon);
        playButton.Click += OnPlayButtonClick;

        //load file content
        // Get the current directory of the running program
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var filePath = Path.Combine(baseDirectory, localfilePath);

        LoadFileContent(filePath);

        // to move left and right up and down
        previousKeyboardState = Keyboard.GetState();
        previousMouseState = Mouse.GetState();
    }
    public void RequestReset()
    {
        ResetRequested?.Invoke(this, EventArgs.Empty);
    }
    private void OnPlayButtonClick()
    {
        RequestReset();
        // Actions to take when the button is clicked
        //InputManager.SetExecute(true,8,8);
        // Concatenate all lines into a single string
        var allText = string.Join("\n", lines);

        // Get the current directory of the running program
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var outputDirectoryName = "../../../Levels"; // Name of your desired output directory
        var directoryPath = Path.Combine(baseDirectory, outputDirectoryName);

        // Create the output directory if it does not exist
        Directory.CreateDirectory(directoryPath);

        // Combine the directory path with the desired file name and write the text to the file
        if (_localfilePath != null)
        {
            var fullPath = Path.Combine(directoryPath,Path.GetFileName(_localfilePath)); 
            File.WriteAllText(fullPath, allText);

            // Log the full path or open the folder containing the file
            Debug.WriteLine($"File written to: {fullPath}");
            RunInterpretor.Execute(fullPath);
        }
        else
        {
            throw new Exception("File path is null");
        }
        
        
    }

    public void LoadFileContent(string filePath)
    {
        // Check if the file exists before trying to read
        if (File.Exists(filePath))
        {
            // Read all lines of the file
            var fileLines = File.ReadAllLines(filePath);

            // Clear current text and reset cursor
            lines.Clear();
            currentLine = 0;
            

            // Add each line from the file to the lines list
            foreach (var line in fileLines) lines.Add(line);
            cursorPosition = lines[currentLine].Length;
            
            // If the file is empty, ensure there is at least one empty line
            if (lines.Count == 0) lines.Add("");
        }
        else
        {
            // If the file does not exist, log this or handle as needed
            Debug.WriteLine($"File not found: {filePath}");
        }
    }

    public void AddCharacter(char character)
    {
        const int margin = 10; // Margin for the text area
        float availableWidth = textAreaRectangle.Width - 2 * margin; // Available width for text
        var maxHeight = Globals.WindowSize.Y - 90; // Maximum height for text lines

        // Calculate maximum number of lines allowed based on maxHeight and line spacing
        var maxLines = ((maxHeight - textAreaRectangle.Y)-70) / spriteFont.LineSpacing;

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
            if (currentLine == maxLines - 1 &&
                spriteFont.MeasureString(lines[currentLine]).X >=
                availableWidth) return; // Do nothing more, prevent overflow

            // Insert character into the current line at the current cursor position
            var newLine = lines[currentLine].Insert(cursorPosition, character.ToString());
            var newSize = spriteFont.MeasureString(newLine);

            // Check if the new line exceeds the available width
            if (newSize.X > availableWidth && lines.Count < maxLines)
            {
                // Split the line at the last space (if exists) or at the current position
                var lastSpace = newLine.LastIndexOf(' ', cursorPosition);
                if (lastSpace > -1)
                {
                    // Create a new line starting from the word after the last space
                    var nextLine = newLine.Substring(lastSpace + 1);
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
        var keyboardState = Keyboard.GetState();
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
            var mouseX = mouseState.X;
            var mouseY = mouseState.Y;

            // Check if the click was inside the text area
            if (textAreaRectangle.Contains(mouseX, mouseY))
            {
                var textStartX = textAreaRectangle.X + 10; // This should match your actual text margin
                var clickX = mouseX - textStartX;
                var clickY = mouseY - textAreaRectangle.Y;

                // Determine which line was clicked
                currentLine = Math.Min(clickY / spriteFont.LineSpacing, lines.Count - 1);
                cursorPosition = 0;

                // Find the nearest character position to the click
                var currentTextLine = lines[currentLine];
                var smallestDistance = float.MaxValue;

                for (var i = 0; i <= currentTextLine.Length; i++)
                {
                    var textUpToCursor = currentTextLine.Substring(0, i);
                    var textSize = spriteFont.MeasureString(textUpToCursor);
                    var distance = Math.Abs(clickX - textSize.X);

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
        var textAreaBackground = new Texture2D(graphicsDevice, 1, 1);
        textAreaBackground.SetData(new[] { Color.LightGray });
        spriteBatch.Draw(textAreaBackground, textAreaRectangle, Color.White); // Use the calculated rectangle
        spriteBatch.Draw(textAreaBackground, numberArea, Color.LightSeaGreen);
        spriteBatch.Draw(textAreaBackground, buttomLine, Color.LightSeaGreen);
        
        DrawBorder(spriteBatch, textAreaRectangle, 2, Color.Black);
        DrawBorder(spriteBatch, numberArea, 2, Color.Black);
        DrawBorder(spriteBatch, buttomLine, 2, Color.Black);
        
        // Set starting position for text (adjust margins as needed)
        var textStartX = textAreaRectangle.X + 10; // 10 pixels from the left edge of the text area
        var textStartY = textAreaRectangle.Y + 10; // 10 pixels from the top edge of the text area
        
        _textBox.Draw(spriteBatch);
        
        
        // Calculate the y-coordinate of the line based on maxLines and line spacing
        var maxLines = ((textAreaRectangle.Height - 70) / spriteFont.LineSpacing);
        var lineY = textAreaRectangle.Y + maxLines * spriteFont.LineSpacing;
        
        // Create a 1x1 pixel texture for the line
        var lineTexture = new Texture2D(graphicsDevice, 1, 1);
        lineTexture.SetData(new[] { Color.Chartreuse });

        // Draw the line from the calculated y-coordinate to the bottom of the text area
        spriteBatch.Draw(lineTexture, new Rectangle(textAreaRectangle.X,(textAreaRectangle.Height - 170), textAreaRectangle.Width, 2), Color.Black);
        
        for (var i = 0; i < lines.Count; i++)
        {
            var lineNumber = i + 1 + "."; // Line numbers start at 1
            var line = lines[i];
            spriteBatch.DrawString(spriteFont, line, new Vector2(textStartX, textStartY + i * spriteFont.LineSpacing),
                Color.Black);

            // Draw line number
            spriteBatch.DrawString(spriteFont, lineNumber,
                new Vector2(textStartX - 40, textStartY + i * spriteFont.LineSpacing), Color.Black);

            // Draw cursor for the current line
            if (i == currentLine)
            {
                var textBeforeCursor = line.Substring(0, cursorPosition);
                var cursorPositionMeasure = spriteFont.MeasureString(textBeforeCursor);
                spriteBatch.DrawString(spriteFont, "|",
                    new Vector2(textStartX + cursorPositionMeasure.X, textStartY + i * spriteFont.LineSpacing),
                    Color.Black);
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
        spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);

        // Draw left line
        spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);

        // Draw right line
        spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.Right, rectangle.Y, thickness, rectangle.Height), color);

        // Draw bottom line
        spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Bottom, rectangle.Width, thickness), color);
        
        
    }
}