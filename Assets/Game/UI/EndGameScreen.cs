
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : UIScreen
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField, SceneName] private string _mainMenuSceneName = "MainMenu";

    private void OnEnable()
    {
        _mainMenuButton.onClick.AddListener(MainMenuButtonClick);
    }

    private void OnDisable()
    {
        _mainMenuButton.onClick.RemoveListener(MainMenuButtonClick);
    }

    private void MainMenuButtonClick()
    {
        Game.SceneManager.LoadScene(_mainMenuSceneName);
    }
}
