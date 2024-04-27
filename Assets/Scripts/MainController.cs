using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Jobs;
using UnityEngine;

internal class MainController : MonoBehaviour
{
    [SerializeField] private UIController _uiController;
    [SerializeField] private float _levelTimeLimit;
    [SerializeField] private Player _player;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private FinishPoint _finishPoint;
    [SerializeField] private List<Enemy> _enemies;  
    [SerializeField] private List<Point> _spawnPoints;
    
    private float _timeLeft;
    private int _attempt;
    private string _saveFilePath;


    void Start()
    {
        _saveFilePath = Application.persistentDataPath + "/gameState.json";
        _uiController.GameStartClicked += StartGame;
        _uiController.SaveGameClicked += SaveGame;
        _uiController.LoadGameClicked += LoadGame;
        _uiController.PauseGameClicked += GamePaused;
        _uiController.ResumeGameClicked += GameResumed;
        _finishPoint.GameWin += () => GameOver(true);
        
        foreach (Enemy enemy in _enemies)
        {
            enemy.TargetCatched += () => GameOver(false);
            enemy.SetTarget(_player.transform);
        }
    }

    private void PrepareField()
    {
        _player.transform.position = _startPoint.position;
        foreach(Point point in _spawnPoints)
        {
            Enemy enemy = GetInactiveEnemy();
            enemy.transform.position = point.transform.position;
            enemy.SetEnemyType(point.enemyType);
            enemy.gameObject.SetActive(true);
            if(point.enemyType == EnemyType.Patrol)
            {
                enemy.SetPatrolRoute(point.patrolRoute);
            }
        }
    }

    private void StartGame()
    {
        PrepareField();
        _timeLeft = _levelTimeLimit;
        StartCoroutine("LevelTimer");
        _player.StartGame();
        foreach (Enemy enemy in _enemies)
        {
            enemy.StartGame();
        }
    }

    private void GameOver(bool res)
    {
        _player.StopGame();
        StopCoroutine("LevelTimer");
        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].StopGame();
            _enemies[i].gameObject.SetActive(false);
        }
        _uiController.GameOver(res);
        _uiController.UpdateAttemptCounter(++_attempt);
    }

    private void GamePaused()
    {
        _player.StopGame();
        foreach (Enemy enemy in _enemies)
        {
            enemy.StopGame();
        }
        StopCoroutine("LevelTimer");
    }

    private void GameResumed()
    {
        _player.StartGame();
        foreach (Enemy enemy in _enemies)
        {
            if(!enemy.gameObject.activeSelf)
            {
                enemy.gameObject.SetActive(true);
            }
            enemy.StartGame();
        }
        StartCoroutine("LevelTimer");
    }

    private void SaveGame()
    {
        GameState gameState = new GameState();
        gameState.cameraTransform = TransformSerializable.GetTransSer(Camera.main.transform);
        gameState.playerTransform = TransformSerializable.GetTransSer(_player.transform);
        gameState.enemiesTransform = new TransformSerializable[_enemies.Count];
        for (int i = 0; i < _enemies.Count; i++)
        {
            gameState.enemiesTransform[i] = TransformSerializable.GetTransSer(_enemies[i].transform);
        }
        gameState.timeLeft = _timeLeft;
        gameState.attempt = _attempt;
        GameStateSaver.Save(gameState, _saveFilePath);
    }

    private void LoadGame()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].StopGame();
            _enemies[i].gameObject.SetActive(false);
        }
        GameState state = GameStateSaver.Load(_saveFilePath);
        TransformSerializable.SetTransform(Camera.main.transform, state.cameraTransform);
        TransformSerializable.SetTransform(_player.transform, state.playerTransform);
        _attempt = state.attempt;
        _timeLeft = state.timeLeft;
        _player.StartGame();
        foreach (var item in state.enemiesTransform)
        {
            Enemy enemy = GetInactiveEnemy();
            TransformSerializable.SetTransform(enemy.transform, item);
        }
        GameResumed();
    }

    private Enemy GetInactiveEnemy()
    {
        foreach (var enemy in _enemies) 
        { 
            if(!enemy.gameObject.activeSelf)
            {
                return enemy;
            }
        }
        return null;
    }
        
    private IEnumerator LevelTimer()
    {
        while (_timeLeft > 0f)
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0f)
            {
                _uiController.UpdateTimeCounter(0);
                GameOver(false);
                yield break;
            }
            _uiController.UpdateTimeCounter(_timeLeft);
            yield return null;
        }
    }
}
