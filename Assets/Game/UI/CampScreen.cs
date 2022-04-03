
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private Image _kidImage;
    [SerializeField] private Image _momImage;
    [SerializeField] private Image _dadImage;

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
        
        StartCoroutine(EndDayCoroutine());
    }

    private IEnumerator EndDayCoroutine()
    {
        yield return null;
        
        var charactersDiedToday = KillStarvingCharacters();
        if (charactersDiedToday.Count > 0)
        {
            foreach (var characterName in charactersDiedToday)
            {
                yield return StartCoroutine(CharacterDeathAnimation(characterName));
            }
        }
        
        if (hasAliveCharacters())
        {
            _campSystem.CurrentDay++;
            Hide();
            uiController.ShowUIElement<GameplayUIScreen>();
        }
        else
        {
            Hide();
            uiController.ShowUIElement<EndGameScreen>();
        }
    }
    
    private const float DeathAnimationFadeDuration = 1f;
    private IEnumerator CharacterDeathAnimation(string characterName)
    {
        switch (characterName)
        {
            case "Dad":
                _dadImage.DOFade(0f, DeathAnimationFadeDuration);
                yield return new WaitForSeconds(DeathAnimationFadeDuration);
                _dadImage.enabled = false;
                break;

            case "Mom":
                _momImage.DOFade(0f, DeathAnimationFadeDuration);
                yield return new WaitForSeconds(DeathAnimationFadeDuration);
                _momImage.enabled = false;
                break;

            case "Kid":
                _kidImage.DOFade(0f, DeathAnimationFadeDuration);
                yield return new WaitForSeconds(DeathAnimationFadeDuration);
                _kidImage.enabled = false;
                break;
            
            default:
                break;
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

    private void updateCurrentFoodText()
    {
        _foodText.text = $"Food: {_campSystem.CurrentFood.ToString()}";
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

    private List<string> KillStarvingCharacters()
    {
        var result = new List<string>();
        foreach (var pair in _campSystem.Characters)
        {
            if (!pair.Value.IsFed)
            {
                pair.Value.IsAlive = false;
                result.Add(pair.Value.Name);
            }
        }

        return result;
    }

}
