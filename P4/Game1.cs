﻿using System;
using System.IO;

namespace P4;
using P4.HomeScreen;
public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Terminal _terminal;
    private StateManager _stateManager;
    private UIManager _uiManager;
    private IScreen _currentScene;
    private LevelSelectionScene _levelSelectionScene;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Set the game to run in full screen
        //_graphics.IsFullScreen = true;


        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height -20;
        _graphics.ApplyChanges();
        Globals.WindowSize = new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        _stateManager = new StateManager();
    }

    protected override void Initialize()
    {
        // Specifies the window size - For fullscreen mode - comment out from here
           Globals.WindowSize = new(1000, 640);
          _graphics.PreferredBackBufferWidth = Globals.WindowSize.X;
          _graphics.PreferredBackBufferHeight = Globals.WindowSize.Y;
           _graphics.ApplyChanges();
           //to here

        Globals.Content = Content;
        Globals.spriteFont = Content.Load<SpriteFont>("TypeFont");
        
        Globals.graphicsDevice = GraphicsDevice;
        
        Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        Color[] colorData = { Color.White }; // Create a 1x1 white pixel
        pixel.SetData(colorData);
        Globals.Pixel = pixel;
        
        
        base.Initialize();
        _uiManager = new UIManager();
        HomeScene homeScreen = new HomeScene();
        _levelSelectionScene = new LevelSelectionScene();
        
        _stateManager.ScreenChanged += OnScreenChanged;
        _uiManager.HomeClicked += () => _stateManager.ChangeScreen(homeScreen);
        homeScreen.OnPlayClicked += () =>
        {
            _stateManager.ChangeScreen(_levelSelectionScene);
            Terminal.resetLines();
        };
        homeScreen.OnQuitClicked += HandleQuitClicked;
        homeScreen.OnResetLevelsClicked += HandleResetLevelsClicked;
        _levelSelectionScene.LevelSelected += (path) =>
        {
            LevelScene levelScene = new LevelScene(path);
            _stateManager.ChangeScreen(levelScene);
            levelScene.ChangeSceneRequested += () =>
            {
                string levelName = Path.GetFileNameWithoutExtension(path);
                _levelSelectionScene.MarkLevelAsCompleted(levelName);
                _stateManager.ChangeScreen(_levelSelectionScene);
            };
            
        };
        
        _stateManager.ChangeScreen(homeScreen); // Start with the home screen
        
        Window.TextInput += TextInputHandler;
    }
    private void OnScreenChanged(IScreen newScreen)
    {
        _currentScene = newScreen;
    }
    private void HandleQuitClicked()
    {
        Exit();  // Exit the game
    }
    private static readonly object fileLock = new object();

    private void HandleResetLevelsClicked()
    {
        string levelsDirectory = Path.Combine(Globals.Content.RootDirectory, "../../../../Levels");
        string initialDirectory = Path.Combine(levelsDirectory, "InitialLevels");
        _levelSelectionScene.ResetCompletedLevels();
        
        if (!Directory.Exists(initialDirectory))
        {
            Console.WriteLine("Source directory does not exist.");
            return;
        }

        if (!Directory.Exists(levelsDirectory))
        {
            Console.WriteLine("Target directory does not exist. Creating it.");
            Directory.CreateDirectory(levelsDirectory);
        }

        // Get all files in the source directory
        string[] sourceFiles = Directory.GetFiles(initialDirectory, "*.txt");

        foreach (string sourceFilePath in sourceFiles)
        {
            string fileName = Path.GetFileName(sourceFilePath);
            string targetFilePath = Path.Combine(levelsDirectory, fileName);

            bool copied = false;
            int retries = 3;

            while (!copied && retries > 0)
            {
                try
                {
                    // Copy the file, overwrite if it already exists
                    lock (fileLock)
                    {
                        File.Copy(sourceFilePath, targetFilePath, true);
                    }
                    copied = true;
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"IOException encountered: {ex.Message}");
                    retries--;
                    System.Threading.Thread.Sleep(100); // Wait for 100 milliseconds before retrying
                }
            }

            if (!copied)
            {
                Console.WriteLine($"Failed to copy {fileName} after multiple attempts.");
            }
        }

        Console.WriteLine("Levels have been reset.");
    }


    private void TextInputHandler(object sender, TextInputEventArgs e)
    {
        if (_currentScene is LevelScene levelScene)
        {
            levelScene.AddCharacterToEditor(e.Character);
        }
    }
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.SpriteBatch = _spriteBatch;

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        Globals.Update(gameTime);
        _uiManager.Update();
        base.Update(gameTime);
        _stateManager.Update(gameTime, Mouse.GetState());
        // Pass the current mouse state to the text editor's update method
        
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.WhiteSmoke);

        // TODO: Add your drawing code here
        _stateManager.Draw(gameTime);
        _spriteBatch.Begin();
        _uiManager.Draw(_spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    protected override void UnloadContent()
    {
        Window.TextInput -= TextInputHandler;
        base.UnloadContent();
    }
}