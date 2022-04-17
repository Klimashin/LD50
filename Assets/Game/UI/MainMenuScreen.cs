using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : UIScreen
{
    [SerializeField, SceneName] private string _gameplaySceneName;
    
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    private void OnEnable()
    {
        Time.timeScale = 1f;
        _startGameButton.onClick.AddListener(OnStartButtonClick);
        _settingsButton.onClick.AddListener(OnSettingsButtonClick);
        _quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(OnStartButtonClick);
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
        _quitButton.onClick.RemoveListener(OnQuitButtonClick);
    }

    private void OnStartButtonClick()
    {
        Game.SceneManager.LoadScene(_gameplaySceneName);
    }

    private void OnSettingsButtonClick()
    {
        UIController.ShowUIElement<SettingsPopup>();
    }

    private void OnQuitButtonClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
