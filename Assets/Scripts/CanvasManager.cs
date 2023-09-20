using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highestScoreText;

    [Header("Timer")]
    [SerializeField] private Image _timerImage;

    [Header("Game Over")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _currentTimerText;
    [SerializeField] private TextMeshProUGUI _gainedTimeText;

    private void OnEnable()
    {
        GameManager.Instance.GameOverAction += ActivateGameOverPanel;
    }

    private void OnDisable()
    {
        GameManager.Instance.GameOverAction -= ActivateGameOverPanel;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateScoreTexts(int currentScore, int highestScore)
    {
        _scoreText.text = "Score: " + currentScore;
        _highestScoreText.text = "Highest Score: " + highestScore;
    }

    public void UpdateTimerImage(float currentTime, float maxTime)
    {
        _timerImage.DOFillAmount(currentTime / maxTime, .15f);
        _currentTimerText.text = (int)currentTime + "s";
    }

    public void UpdateGainedTimeText(float gainedTime)
    {
        _gainedTimeText.text = "+" + (int)gainedTime + "s";

        _gainedTimeText.DOFade(1, .2f).OnComplete(() => 
        {
            _gainedTimeText.DOFade(0, .2f).SetDelay(1f);
        });
    }

    public void ActivateGameOverPanel()
    {
        _gameOverPanel.SetActive(true);
        _gameOverPanel.GetComponent<CanvasGroup>().DOFade(1, .3f);
    }

    public void RetryButtonAction()
    {
        GameManager.Instance.RestartGame();
    }

}
