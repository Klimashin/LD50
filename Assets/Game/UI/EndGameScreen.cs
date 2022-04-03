
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : UIScreen
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField, SceneName] private string _mainMenuSceneName = "MainMenu";
    [SerializeField] private CampSystem _campSystem;
    [SerializeField] private TextMeshProUGUI _daysSurvivedText;

    private void OnEnable()
    {
        _mainMenuButton.onClick.AddListener(MainMenuButtonClick);

        _daysSurvivedText.text = $"Days survived: {_campSystem.CurrentDay.ToString()}";
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
