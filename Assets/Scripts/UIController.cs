using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


internal class UIController : MonoBehaviour
{
    
    [SerializeField] private GameObject _startGamePanel;
    [SerializeField] private GameObject _gamePlayPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private TMP_Text _roundResult;
    [SerializeField] private TMP_Text _timeLeftText;
    [SerializeField] private TMP_Text _attemptText;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _saveGameButton;
    [SerializeField] private Button _loadGameButton;
    [SerializeField] private Button _ResumeGameButton;
        
    public Action GameStartClicked;
    public Action PauseGameClicked;
    public Action ResumeGameClicked;
    public Action SaveGameClicked;
    public Action LoadGameClicked;
    void Start()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _menuButton.onClick.AddListener(PauseGame);
        _ResumeGameButton.onClick.AddListener(ResumeGame);
        _saveGameButton.onClick.AddListener(() => SaveGameClicked?.Invoke());
        _loadGameButton.onClick.AddListener(LoadGame);
    }

    private void StartGame()
    {
        GameStartClicked?.Invoke();
        _startGamePanel.SetActive(false);
    }

    public void GameOver(bool result)
    {
        _roundResult.text = result ? "YOU WIN!" : "YOU LOSE";
        _startGamePanel.SetActive(true);
    }

    private void PauseGame()
    {
        PauseGameClicked?.Invoke();
        _menuButton.onClick.RemoveAllListeners();
        _pausePanel.SetActive(true);
    }

    private void ResumeGame()
    {

        ResumeGameClicked?.Invoke();
        _menuButton.onClick.AddListener(() => PauseGame());
        _pausePanel.SetActive(false);
    }

    private void LoadGame()
    {
        LoadGameClicked?.Invoke();
        _menuButton.onClick.AddListener(() => PauseGame());
        _pausePanel.SetActive(false);
    }
    
    public void UpdateTimeCounter(float time)
    {
        _timeLeftText.text = ($"Time Left: {((int)time)}");
    }

    public void UpdateAttemptCounter(int attempt)
    {
        _attemptText.text =  $"Attempt: {attempt}";
    }
}
