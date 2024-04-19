﻿namespace P4.HomeScreen;

public class LevelScene : IScreen
{
    private TextEditor _textEditor;
    private Terminal _terminal;
    
    public LevelScene(string path)
    {
        InitializeLevel(path);
    }
    public void InitializeLevel(string path)
    {
        _textEditor = new TextEditor(Globals.graphicsDevice, Globals.SpriteBatch, Globals.spriteFont, path);
        _terminal = new Terminal(Globals.graphicsDevice , Globals.SpriteBatch, Globals.spriteFont);
    }
    public void AddCharacterToEditor(char character)
    {
        _textEditor.AddCharacter(character);
    }
    public void Update(GameTime gameTime, MouseState mouseState)
    {
        _textEditor.Update(mouseState);
    }

    public void Draw(GameTime gameTime)
    {
        _textEditor.Draw();
        _terminal.Draw();
    }
    
}