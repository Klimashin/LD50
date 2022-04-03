using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampScreen : UIScreen
{
    [SerializeField] private CampSystem _campSystem;
    [SerializeField] private Button _endDayButton;
    [SerializeField] private Button _dadFeedButton;
    [SerializeField] private Button _momFeedButton;
    [SerializeField] private Button _kidFeedButton;
    [SerializeField] private TextMeshProUGUI _foodText;
    [SerializeField] private TextMeshProUGUI _currentDayText;
    [SerializeField] private GameObject _dadSpeechArea;
    [SerializeField] private GameObject _momSpeechArea;
    [SerializeField] private GameObject _kidSpeechArea;
    [SerializeField] private TextMeshProUGUI _dadSpeech;
    [SerializeField] private TextMeshProUGUI _momSpeech;
    [SerializeField] private TextMeshProUGUI _kidSpeech;
    [SerializeField] private int _timeoutTimeSec = 2;
    

    private void OnEnable()
    {
        setCharactersUnfed();
        updateCurrentDayText();
        hideAllSpeechAreas();
        _endDayButton.onClick.AddListener(OnEndDayButtonClick);
        _dadFeedButton.onClick.AddListener(delegate { OnFeedButtonClick(_dadFeedButton, "Dad"); });
        _momFeedButton.onClick.AddListener(delegate { OnFeedButtonClick(_momFeedButton, "Mom"); });
        _kidFeedButton.onClick.AddListener(delegate { OnFeedButtonClick(_kidFeedButton, "Kid"); });
    }

    private void OnDisable()
    {
        _endDayButton.onClick.RemoveListener(OnEndDayButtonClick);
        _dadFeedButton.onClick.RemoveListener(delegate { OnFeedButtonClick(_dadFeedButton, "Dad"); });
        _momFeedButton.onClick.RemoveListener(delegate { OnFeedButtonClick(_momFeedButton, "Mom"); });
        _kidFeedButton.onClick.RemoveListener(delegate { OnFeedButtonClick(_kidFeedButton, "Kid"); });
    }

    private void Update()
    {
        updateCurrentFoodText();
    }

    private void OnEndDayButtonClick()
    {
        if (!сheckFedCharacters())
        {
            return;
        }

        KillStarvingCharacters();
        
        if (hasAliveCharacters())
        {
            OnDayEnded();
        }
        else
        {
            OnAllCharactersDead();
        }
    }

    private void OnFeedButtonClick(Button button, string characterKey)
    {
        hideAllSpeechAreas();

        if (
            _campSystem.Characters[characterKey].IsAlive
            && !_campSystem.Characters[characterKey].IsFed
            && _campSystem.CurrentFood >= _campSystem.Characters[characterKey].FoodRequired
            )
        {
            StartCoroutine(showCharacterSpeech(characterKey));
            _campSystem.Characters[characterKey].IsFed = true;
            _campSystem.CurrentFood -= _campSystem.Characters[characterKey].FoodRequired;
            button.onClick.RemoveListener(delegate { OnFeedButtonClick(button, characterKey); });
            button.enabled = false;
        }
    }

    private void setCharactersUnfed()
    {

        foreach (var character in _campSystem.Characters)
        {
            if (character.Value.IsAlive)
            {
                character.Value.IsFed = false;
            }
        }
    }

    private void updateCurrentDayText()
    {
        _currentDayText.text = $"Day: {_campSystem.CurrentDay.ToString()}";
    }

    private bool сheckFedCharacters()
    {
        foreach (var character in _campSystem.Characters)
        {
            if (!character.Value.IsAlive)
                continue;
            
            if (character.Value.IsAlive && !character.Value.IsFed)
            {
                if (_campSystem.CurrentFood >= character.Value.FoodRequired)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool hasAliveCharacters()
    {
        foreach (var character in _campSystem.Characters)
        {
            if (character.Value.IsAlive)
            {
                return true;
            }
        }

        return false;

    }

    private void dayCounterInc()
    {
        _campSystem.CurrentDay++;
    }

    private void updateCurrentFoodText()
    {
        _foodText.text = $"Food: {_campSystem.CurrentFood.ToString()}";
    }

    private void OnDayEnded()
    {
        dayCounterInc();
        Hide();
        uiController.ShowUIElement<GameplayUIScreen>();
    }

    private void OnAllCharactersDead()
    {
        Hide();
        uiController.ShowUIElement<EndGameScreen>();
    }

    private IEnumerator showCharacterSpeech(string characterKey)
    {
        string[] CharacterSpeechArray = _campSystem.Characters[characterKey].SpeechArray;
        string speech = CharacterSpeechArray[new System.Random().Next(0, CharacterSpeechArray.Length)];
        Debug.Log(speech);

        switch (characterKey)
        {
            case "Dad":
                _dadSpeech.text = speech;
                _dadSpeechArea.SetActive(true);
                yield return new WaitForSeconds(_timeoutTimeSec);
                _dadSpeechArea.SetActive(false);
                break;

            case "Mom":
                _momSpeech.text = speech;
                _momSpeechArea.SetActive(true);
                yield return new WaitForSeconds(_timeoutTimeSec);
                _momSpeechArea.SetActive(false);
                break;

            case "Kid":
                _kidSpeech.text = speech;
                _kidSpeechArea.SetActive(true);
                yield return new WaitForSeconds(_timeoutTimeSec);
                _kidSpeechArea.SetActive(false);
                break;
            default:
                break;
        }

    }

    private void hideAllSpeechAreas()
    {
        _dadSpeechArea.SetActive(false);
        _momSpeechArea.SetActive(false);
        _kidSpeechArea.SetActive(false);
    }

    private void KillStarvingCharacters()
    {
        foreach (var pair in _campSystem.Characters)
        {
            if (!pair.Value.IsFed)
            {
                pair.Value.IsAlive = false;
                
                // TODO: play character Death sound
            }
        }
    }

}
