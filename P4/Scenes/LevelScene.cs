using System;
using P4.Managers;

namespace P4.HomeScreen;

public class LevelScene : IScreen
{
    private TextEditor _textEditor;
    private Terminal _terminal;
    private GameManager _gameManager;
    
    public LevelScene(string path)
    {
        InitializeLevel(path);
    }
    public void InitializeLevel(string path)
    {
        _textEditor = new TextEditor(Globals.graphicsDevice, Globals.SpriteBatch, Globals.spriteFont, path);
        _terminal = new Terminal(Globals.graphicsDevice , Globals.SpriteBatch, Globals.spriteFont);
        _gameManager = new GameManager();
        
        _textEditor.ResetRequested += TextEditor_ResetRequested;
    }
    private void TextEditor_ResetRequested(object sender, EventArgs e)
    {
        // Reset the GameManager and any other objects that need to be reset
        _gameManager = new GameManager(); // Reinitialize GameManager
        // Add logic to reset other objects if needed
    }
    public void AddCharacterToEditor(char character)
    {
        _textEditor.AddCharacter(character);
    }
    public void Update(GameTime gameTime, MouseState mouseState)
    {
        _textEditor.Update(mouseState);
        _gameManager.Update();
    }

    public void Draw(GameTime gameTime)
    {
        _textEditor.Draw();
        _terminal.Draw();
        _gameManager.Draw();
    }
    
}