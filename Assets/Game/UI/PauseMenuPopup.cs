using UnityEngine;
using UnityEngine.UI;

public class PauseMenuPopup : UIPopup
{
    [SerializeField, SceneName] private string _mainMenuSceneName;
    [SerializeField] private Button _mainMenuButton;

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
