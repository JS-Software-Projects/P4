using System;
using System.Collections.Generic;
using p4.Actors;
using P4.Actors.Towers;
using P4.managers;

namespace P4.Managers;

public class GameManager
{
    private static Map _map;
    private Hero _hero;
    public static List<BasicTower> _tower;
    List<Vector2> enemyPositions = new List<Vector2>();
    public int _circleCount = 0;
    public static int _level;
    private Button _playButton;
    public event Action OnHomeClicked;
    private Rectangle _backgroundBox;
    private Vector2 _textPosition;
    private string WonText = "You have completed level: ";
    private Texture2D _buttonTexture;

    public GameManager(int level)
    {
        _buttonTexture = new Texture2D(Globals.graphicsDevice, 1, 1);
        _buttonTexture.SetData(new[] { Color.White });
        _level = level;
        WonText += level;
        _tower = new List<BasicTower>();
        _map = new(level);
        if (level != 1)
        {
            CreateHero();
        }
        //var position = _map.MapToScreen(0, 2);
        //_hero = new(Globals.Content.Load<Texture2D>("hero"),position);
        InitializeBackground();
        InitializeButtons();
    }
    public void CreateHero()
    {
        var position = _map.MapToScreen(0, 2);
        _hero = new(Globals.Content.Load<Texture2D>("hero"),position);
        Pathfinder.Init(_map, _hero);
    }

    public void checkHeroCircleHit()
    {
        foreach (var tile in _map.Tiles)
        {
            //+32 since hero and circle are not centered the same way
            if (tile.CircleExist() && _hero.Position.X+32 == tile.getCircle().Position.X && _hero.Position.Y+32 == tile.getCircle().Position.Y)
            {
                var x = _hero.Position.X / Globals.TileSize;
                var y = _hero.Position.Y / Globals.TileSize;
                _map.Tiles[(int)x, (int)y].removeCircle();
                break;
            }
        }
    }
    public static BasicTower AddTower(double x,double y)
    {
        if (_map.Tiles[(int)x-1, (int)y-1].TowerPlaced)
        {
            throw new ArgumentException("Tower already placed on this tile");
        }

        if (_map.Tiles[(int)x-1, (int)y-1]._boundry.Count == 2)
        {
            throw new Exception("Tower cannot be placed on path tile");
        }
    
        
        BasicTower tower =  new(Globals.Content.Load<Texture2D>("Cannon"), new Vector2((float)x*Globals.TileSize, (float)y*Globals.TileSize), Color.White);
        _tower.Add(tower);
        if (_map.Tiles[(int) x-1, (int) y-1].CircleExist()){
            _map.Tiles[(int)x-1, (int)y-1].removeCircle();
        }
        _map.Tiles[(int)x-1, (int)y-1].Blocked = true;
        _map.Tiles[(int)x-1, (int)y-1].TowerPlaced = true;
        return tower;
    }

    public static bool HeroMove(int x, int y)
    {
        if (_map.Tiles[x-1,y-1].Blocked)
        {
            return false;
        }
        InputManager.SetExecute(true, x, y);
        return true;
    }
    public void CheckCircles()
    {
        var count = 0;
        if(_map.Tiles != null)
        {
            foreach (var tile in _map.Tiles)
            {
                if (tile.CircleExist())
                {
                    count++;
                }
            }
        }
        //Sandbox for the level 3+
        if (_level != 1 && _level != 2)
        {
            count = -1;
        }
        _circleCount = count;
    }
    public void CheckScore()
    {
        if (_circleCount == 0)
        {
            Terminal.SetError(false,"You Win");
        }
    }
    private void InitializeBackground()
    {
        int boxWidth = 400;
        int boxHeight = 200;
        int centerX = Globals.WindowSize.X / 2;
        int centerY = Globals.WindowSize.Y / 2;
    
        _backgroundBox = new Rectangle(centerX - boxWidth / 2, centerY - boxHeight / 2, boxWidth, boxHeight);
        Vector2 textSize = Globals.spriteFont.MeasureString(WonText);
        _textPosition = new Vector2(centerX - textSize.X / 2, centerY - textSize.Y / 2 - 50);
    }
    private void InitializeButtons()
    {
        // Define the button colors
        Color textColor = Color.White;
        Color backgroundColor = Color.Gray;
        Color hoverColor = Color.DarkGray;
            
        // Create the Play button
        _playButton = new Button(new Rectangle(Globals.WindowSize.X / 2 - 100, Globals.WindowSize.Y / 2, 200, 50), "Go back to Levels", Globals.spriteFont, _buttonTexture, textColor, backgroundColor, hoverColor);
        _playButton.Click += PlayButton_Click;
    }
    private void PlayButton_Click()
    {
        // Code to go back
        OnHomeClicked?.Invoke();
    }

    

    public void Update(MouseState mouseState)
    {
        _map.Update();
       
        if (_hero != null)
        {
            _hero.Update();
            enemyPositions.Clear();
            enemyPositions.Add(_hero.Position);
            foreach (var tower in _tower)
            {
             //   if (tower != null)
                  //  tower.Update(enemyPositions);
            }   
            checkHeroCircleHit();
        }
        
        CheckCircles();
        CheckScore();
        if (_circleCount == 0){
            _playButton.Update(mouseState);
        }
    }

    public void Draw()
    {
        Globals.SpriteBatch.Begin();
        _map.Draw();
        if (_hero != null)
            _hero.Draw();
        foreach (var tower in _tower)
        {
            if (tower != null)
                tower.Draw();    
        }
        if (_circleCount == 0){
            
            // Draw the background box
            Globals.SpriteBatch.Draw(_buttonTexture, _backgroundBox, Color.Black * 0.7f);
            // Draw the "Welcome" text
            Globals.SpriteBatch.DrawString(Globals.spriteFont, WonText, _textPosition, Color.White);
            _playButton.Draw(Globals.SpriteBatch);
        }
        Globals.SpriteBatch.End();
    }
}