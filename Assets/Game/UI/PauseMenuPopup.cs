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
        Time.timeScale = 1;
        _mainMenuButton.onClick.RemoveListener(MainMenuButtonClick);
    }

    protected override void OnPostHide()
    {
        base.OnPostHide();
        Time.timeScale = 1;
    }

    protected override void OnPreShow()
    {
        base.OnPreShow();
        Time.timeScale = 0;
    }
    
    private void MainMenuButtonClick()
    {
        Game.SceneManager.LoadScene(_mainMenuSceneName);
    }
}