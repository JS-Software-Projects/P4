﻿using System;
using System.Collections.Generic;
using p4.Actors;
using P4.Actors.Towers;
using P4.managers;

namespace P4.Managers;

public class GameManager
{
    private static Map _map;
    private readonly Hero _hero;
    public static List<BasicTower> _tower = new List<BasicTower>();
    List<Vector2> enemyPositions = new List<Vector2>();

    public GameManager()
    {
        _map = new();
        var position = _map.MapToScreen(0, 3);
        _hero = new(Globals.Content.Load<Texture2D>("hero"),position);
        Pathfinder.Init(_map, _hero);
        //BasicTower tower = new BasicTower(Globals.Content.Load<Texture2D>("Cannon"), new Vector2(5*Globals.TileSize, 5*Globals.TileSize), Color.White);
        //_tower.Add(tower);
    }
    public static void AddTower(BasicTower tower)
    {
        _tower.Add(tower);
    }

    public static void HeroMove(int x, int y)
    {
/*
        foreach (var tile in _map.Tiles)
        {
            Console.WriteLine("tileX: "+tile._mapX+" tileY: "+tile._mapY);
            Console.WriteLine("count: "+tile._boundry.Count+" Blocked? "+tile.Blocked +" Path "+tile.Path);

        }*/
  
      
      if (_map.Tiles[x-1, y-1]._boundry.Count != 2)
        {
            Terminal.SetError(true,"Cannot move hero to Tile : " + x + " " + y + ". Tile is not a path");
            return;
        }
        
        InputManager.SetExecute(true, x, y);
    }
    

    public void Update()
    {
        _map.Update();
        _hero.Update();
        enemyPositions.Clear();
        enemyPositions.Add(_hero.Position);
        foreach (var tower in _tower)
        {
            if (tower != null)
                tower.Update(enemyPositions);
        }
        
    }

    public void Draw()
    {
        Globals.SpriteBatch.Begin();
        _map.Draw();
        _hero.Draw();
        foreach (var tower in _tower)
        {
            if (tower != null)
                 tower.Draw();    
        }
        Globals.SpriteBatch.End();
    }
}