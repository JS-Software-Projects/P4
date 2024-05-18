using System;
using System.Text.RegularExpressions;
using P4.Managers;

namespace P4.HomeScreen;

public class LevelScene : IScreen
{
    private TextEditor _textEditor;
    private Terminal _terminal;
    private GameManager _gameManager;
    private readonly string _path;
    public event Action ChangeSceneRequested;
    
    public LevelScene(string path)
    {
        _path = path;
        InitializeLevel(path);
    }
    private void InitializeLevel(string path)
    {
        _textEditor = new TextEditor(Globals.graphicsDevice, Globals.SpriteBatch, Globals.spriteFont, path);
        _terminal = new Terminal(Globals.graphicsDevice , Globals.SpriteBatch, Globals.spriteFont);
        _gameManager = new GameManager(GetNumberFromPath(path));
        
        _textEditor.ResetRequested += TextEditor_ResetRequested;
        _gameManager.OnHomeClicked += goBack;
    }
    public static int GetNumberFromPath(string path)
    {
        // Use regular expression to find the number
        Match match = Regex.Match(path, @"\d+");
        if (match.Success)
        {
            return int.Parse(match.Value);
        }
        throw new FormatException("No number found in the path.");
    }

    private void goBack()
    {
        ChangeSceneRequested?.Invoke();
        Terminal.resetLines();
    }
    private void TextEditor_ResetRequested(object sender, EventArgs e)
    {
        _gameManager = new GameManager(GetNumberFromPath(_path)); // Reinitialize GameManager
        _gameManager.OnHomeClicked += goBack;
        Terminal.resetLines();
    }
    public void AddCharacterToEditor(char character)
    {
        _textEditor.AddCharacter(character);
    }
    public void Update(GameTime gameTime, MouseState mouseState)
    {
        _textEditor.Update(mouseState);
        _gameManager.Update(mouseState);
    }

    public void Draw(GameTime gameTime)
    {
        _textEditor.Draw();
        _terminal.Draw();
        _gameManager.Draw();
    }
    
}