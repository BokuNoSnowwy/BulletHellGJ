using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState{
    Pause,
    Game
}

public class GameManager : MonoBehaviour
{
    private GameState _gameState;
    private float _timerGame;
    
    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (_gameState == GameState.Game)
        {
            _timerGame += Time.deltaTime;
        }
    }

    private void StartGame()
    {
        _gameState = GameState.Game;
        _timerGame = 0;
    }

    public float TimerGame => _timerGame;

    public GameState GameState => _gameState;
}
