using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool GameOver { get; private set; }
    
    private int _currentScore;

    [Header("Spawn")]
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private CircleCollider2D _ballSpawnArea;

    [Header("Session Timing")]
    [SerializeField] private float _sessionMaxTime;
    [SerializeField] private float _timeGainOnKills;
    private float _sessionTimer;

    public Action PlayerDestroyedBallAction { get; internal set; }
    public Action GameOverAction { get; internal set; }

    private void OnEnable()
    {
        PlayerDestroyedBallAction += PlayerDestroyedBall;
        GameOverAction += GameOverActions;
    } 

    private void OnDisable()
    {
        PlayerDestroyedBallAction -= PlayerDestroyedBall;
        GameOverAction -= GameOverActions;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnBall();

        CanvasManager.Instance.UpdateScoreTexts(_currentScore, PlayerPrefs.GetInt("HighestScore", 0));

        _sessionTimer = _sessionMaxTime;
    }

    private void Update()
    {
        if (GameOver) return;

        if (_sessionTimer > 0)
        {
            _sessionTimer -= Time.deltaTime;

            CanvasManager.Instance.UpdateTimerImage(_sessionTimer, _sessionMaxTime);
        }
        else
        {
            GameOverAction?.Invoke();
        }
    }

    private void GameOverActions()
    {
        GameOver = true;
    }

    private void PlayerDestroyedBall()
    {
        _currentScore++;

        if (_currentScore > PlayerPrefs.GetInt("HighestScore", 0))
            PlayerPrefs.SetInt("HighestScore", _currentScore);

        CanvasManager.Instance.UpdateScoreTexts(_currentScore, PlayerPrefs.GetInt("HighestScore", 0));

        _sessionTimer += _timeGainOnKills;

        if (_sessionTimer > _sessionMaxTime) _sessionTimer = _sessionMaxTime;

        CanvasManager.Instance.UpdateGainedTimeText(_timeGainOnKills);

        SpawnBall();
    }

    private void SpawnBall()
    {
        if (GameOver) return;

        Vector2 spawnPosition = Player.Instance.transform.position;

        while (Vector2.Distance(spawnPosition, Player.Instance.transform.position) < 1.5f)
        {
            spawnPosition = new Vector2(UnityEngine.Random.Range(_ballSpawnArea.bounds.min.x, _ballSpawnArea.bounds.max.x),
                UnityEngine.Random.Range(_ballSpawnArea.bounds.min.y, _ballSpawnArea.bounds.max.y));
        }

        GameObject ball =  Instantiate(_ballPrefab, spawnPosition, Quaternion.identity);
        ball.transform.localScale = Vector2.zero;
        ball.transform.DOScale(1, .2f).SetEase(Ease.OutBack);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
