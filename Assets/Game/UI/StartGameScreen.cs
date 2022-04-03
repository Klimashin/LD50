
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGameScreen : UIScreen
{
    [SerializeField] private CampSystem _campSystem;
    [SerializeField] private Button _startGameButton;

    public override void OnStart()
    {
        base.OnPostShow();
        
        _campSystem.Reset();
        
        _startGameButton.onClick.AddListener(OnStartGameButtonClick);

        StartCoroutine(GameplayInitializer());
    }

    private void OnStartGameButtonClick()
    {
        uiController.ShowUIElement<GameplayUIScreen>();
        Hide();
    }

    private IEnumerator GameplayInitializer()
    {
        _startGameButton.interactable = false;

        var generator = FindObjectOfType<LevelGenerator>();

        yield return generator.Generate();

        _startGameButton.interactable = true;
    }
}
