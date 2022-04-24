using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContinueGamePopup : UIPopup
{
    [SerializeField, SceneName] private string _gameplaySceneName;
    
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TextMeshProUGUI _seedText;

    private WorldData _worldData;
    
    protected override void OnPreShow()
    {
        base.OnPreShow();

        _worldData = Game.FileStorage.Get<WorldData>("worldData");
        _seedText.text = _worldData.WorldSeed.ToString();
    }
    
    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(OnStartButtonClick);
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(OnStartButtonClick);
    }

    private void OnStartButtonClick()
    {
        var loadingParams = new Dictionary<string, object> { {"worldData", _worldData} };
        Game.SceneManager.LoadScene(_gameplaySceneName, loadingParams);
    }
}
