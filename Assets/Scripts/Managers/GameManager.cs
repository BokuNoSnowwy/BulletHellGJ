using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum GameState{
    Pause,
    Game
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private GameState _gameState;
    private float _timerGame;
    private UnityEvent<GameState> _onGameStateChanged = new UnityEvent<GameState>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

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

    public void SetGameState(GameState gameState)
    {
        _gameState = gameState;
        _onGameStateChanged.Invoke(gameState);
    }

    public void AddGameStateChangeListener(UnityAction<GameState> action)
    {
        _onGameStateChanged.AddListener(action);
    }

    public float TimerGame => _timerGame;

    public GameState GameState => _gameState;
}
