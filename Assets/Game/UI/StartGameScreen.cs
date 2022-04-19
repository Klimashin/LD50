
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
        
        var generatorSettingsHandle =  Addressables.LoadAssetAsync<LevelGeneratorSettings>("Assets/GeneratorSettings.asset");
        while (!generatorSettingsHandle.IsDone)
        {
            yield return null;
        }

        _generator = new LevelGenerator(generatorSettingsHandle.Result);
        
        _startGameButton.interactable = false;

        yield return null;
        yield return null;
        yield return null;
        
        var seed = 12345;
        yield return _generator.Generate(seed);
        
        GenerationProgressFill.transform.parent.gameObject.SetActive(false);
        _startGameButton.gameObject.SetActive(true);

        _startGameButton.interactable = true;
    }
}
