namespace P4;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private TextEditor _textEditor;

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
        base.Initialize();
        _textEditor = new TextEditor(GraphicsDevice, _spriteBatch, Content.Load<SpriteFont>("TypeFont"));
        Window.TextInput += TextInputHandler;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.SpriteBatch = _spriteBatch;

        // TODO: use this.Content to load your game content here
    }

    private void TextInputHandler(object sender, TextInputEventArgs e)
    {
        _textEditor.AddCharacter(e.Character);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        Globals.Update(gameTime);
        base.Update(gameTime);
        // Pass the current mouse state to the text editor's update method
        if (_textEditor != null) // Make sure your text editor is initialized
            _textEditor.Update(Mouse.GetState());
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.WhiteSmoke);

        // TODO: Add your drawing code here
        _textEditor.Draw();
        base.Draw(gameTime);
    }

    protected override void UnloadContent()
    {
        Window.TextInput -= TextInputHandler;
        base.UnloadContent();
    }
}