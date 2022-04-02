using UnityEngine;
using UnityEngine.UI;

public class PauseMenuPopup : UIPopup
{
    [SerializeField, SceneName] private string _mainMenuSceneName;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _campTestButton;

    private void OnEnable()
    {
        _mainMenuButton.onClick.AddListener(MainMenuButtonClick);
        _campTestButton.onClick.AddListener(OpenCampScreen);
    }

    private void OpenCampScreen()
    {
        Hide();
        uiController.GetUIElement<GameplayUIScreen>().Hide();
        uiController.ShowUIElement<CampScreen>();
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
