
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGameScreen : UIScreen
{
    [SerializeField] private CampSystem _campSystem;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Image GenerationProgressFill;

    private LevelGenerator _generator;
    
    public override void OnStart()
    {
        _campSystem.Reset();
        
        _startGameButton.onClick.AddListener(OnStartGameButtonClick);

        _generator = FindObjectOfType<LevelGenerator>();

        StartCoroutine(GameplayInitializer());
    }

    private void OnStartGameButtonClick()
    {
        UIController.ShowUIElement<GameplayUIScreen>();
        Hide();
    }

    private void Update()
    {
        if (_generator != null)
            GenerationProgressFill.fillAmount = _generator.WorldGenerationProgress;
    }

    private IEnumerator GameplayInitializer()
    {
        Time.timeScale = 1;
        
        _startGameButton.interactable = false;

        yield return null;
        yield return null;
        yield return null;

        yield return _generator.Generate();
        
        GenerationProgressFill.transform.parent.gameObject.SetActive(false);
        _startGameButton.gameObject.SetActive(true);

        _startGameButton.interactable = true;
    }
}
