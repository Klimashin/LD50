
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
        _startGameButton.onClick.AddListener(OnStartGameButtonClick);

        var worldData = Game.SceneManager.CurrentScene.GetSceneParam<WorldData>("worldData");
        if (worldData != null)
        {
            StartCoroutine(WorldRestoration(worldData));
        }
        else
        {
            StartCoroutine(WorldGeneration(Game.SceneManager.CurrentScene.GetSceneParam<int>("worldSeed")));
        }
    }

    private void OnStartGameButtonClick()
    {
        UIController.ShowUIElement<CampScreen>();
        Hide();
    }

    private void Update()
    {
        if (_generator != null)
            GenerationProgressFill.fillAmount = _generator.WorldGenerationProgress;
    }

    private IEnumerator WorldGeneration(int seed)
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
        
        yield return _generator.Generate(seed);
        
        GenerationProgressFill.transform.parent.gameObject.SetActive(false);
        _startGameButton.gameObject.SetActive(true);
        _campSystem.Reset();

        _startGameButton.interactable = true;
    }

    private IEnumerator WorldRestoration(WorldData world)
    {
        var counterMax = world.WorldObjectsData.Count;
        var counter = 0;
        foreach (var worldObjectData in world.WorldObjectsData)
        {
            var worldObjectHandle = worldObjectData.PrefabAssetAddress.InstantiateAsync();
            while (!worldObjectHandle.IsDone)
            {
                yield return null;
            }
            
            var worldObject = worldObjectHandle.Result.GetComponent<WorldObject>();
            worldObject.Initialize(worldObjectData);

            counter++;

            GenerationProgressFill.fillAmount = counter / (float)counterMax;
        }
        
        yield return null;
        
        GenerationProgressFill.transform.parent.gameObject.SetActive(false);
        _startGameButton.gameObject.SetActive(true);
        _campSystem.InitFromWorldData(world);
        
        _startGameButton.interactable = true;
    }
}
