using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewGamePopup : UIPopup
{
    [SerializeField, SceneName] private string _gameplaySceneName;
    
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TMP_InputField _seedInput;
    [SerializeField] private Button _newSeedButton;

    protected override void OnPreShow()
    {
        base.OnPreShow();
        
        if (string.IsNullOrEmpty(_seedInput.text))
            SetNewSeed();
    }

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(OnStartButtonClick);
        _newSeedButton.onClick.AddListener(SetNewSeed);
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(OnStartButtonClick);
        _newSeedButton.onClick.RemoveListener(SetNewSeed);
    }

    private void SetNewSeed()
    {
        _seedInput.text = Mathf.Abs(Guid.NewGuid().GetHashCode()).ToString();
    }

    private void OnStartButtonClick()
    {
        var seed = Int32.Parse(_seedInput.text);
        var sceneSettings = new Dictionary<string, object> { { "worldSeed", seed } };
        Game.SceneManager.LoadScene(_gameplaySceneName, sceneSettings);
    }
}
