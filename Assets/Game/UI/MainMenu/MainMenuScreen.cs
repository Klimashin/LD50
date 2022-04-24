using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : UIScreen
{
    [SerializeField, SceneName] private string _gameplaySceneName;
    
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private FireShadow _fireShadow;

    protected override void OnPostShow()
    {
        base.OnPostShow();
        
        _fireShadow.enabled = true;
        SetMenuButtonsActive(true);
        _continueGameButton.gameObject.SetActive(Game.FileStorage.Get<WorldData>("worldData") != null);
    }

    protected override void OnPreShow()
    {
        base.OnPreShow();

        SetMenuButtonsActive(false);
    }

    private void SetMenuButtonsActive(bool buttonsActive)
    {
        _continueGameButton.gameObject.SetActive(buttonsActive);
        _startGameButton.gameObject.SetActive(buttonsActive);
        _settingsButton.gameObject.SetActive(buttonsActive);
        _quitButton.gameObject.SetActive(buttonsActive);
    }

    private void OnEnable()
    {
        Time.timeScale = 1f;
        _startGameButton.onClick.AddListener(OnStartButtonClick);
        _continueGameButton.onClick.AddListener(OnContinueButtonClick);
        _settingsButton.onClick.AddListener(OnSettingsButtonClick);
        _quitButton.onClick.AddListener(OnQuitButtonClick);
    }
    
    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(OnStartButtonClick);
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
        _quitButton.onClick.RemoveListener(OnQuitButtonClick);
    }

    private void OnContinueButtonClick()
    {
        UIController.ShowUIElement<ContinueGamePopup>();
    }

    private void OnStartButtonClick()
    {
        UIController.ShowUIElement<NewGamePopup>();
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
