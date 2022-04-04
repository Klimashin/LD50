
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
    [SerializeField] private int _timeoutTimeSec = 5;
    [SerializeField] private Image _kidImage;
    [SerializeField] private Image _momImage;
    [SerializeField] private Image _dadImage;
    [SerializeField] private Image _kidFoodImage;
    [SerializeField] private Image _momFoodImage;
    [SerializeField] private Image _dadFoodImage;

    [SerializeField] private GameObject _uiCanvas;
    GraphicRaycaster _uiRaycaster;
    PointerEventData _clickData;
    List<RaycastResult> _clickResults;

    public override void OnCreate()
    {
        base.OnCreate();

        _uiRaycaster = _uiCanvas.GetComponent<GraphicRaycaster>();
        _clickData = new PointerEventData(EventSystem.current);
        _clickResults = new List<RaycastResult>();
        _endDayButton.onClick.AddListener(OnEndDayButtonClick);
    }

    private void OnEnable()
    {
        setCharactersUnfed();
        updateCurrentDayText();
        hideAllSpeechAreas();
        checkEndDayButton();
    }

    private void Update()
    {
        updateCurrentFoodText();
        checkEndDayButton();
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            GetUiElementsClicked();
        }
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
            {
                var images = _dadImage.GetComponentsInChildren<Image>();
                foreach (var image in images)
                {
                    image.DOFade(0f, DeathAnimationFadeDuration);
                }
                yield return new WaitForSeconds(DeathAnimationFadeDuration);
                _dadImage.enabled = false;
                break;
            }

            case "Mom":
            {
                var images = _momImage.GetComponentsInChildren<Image>();
                foreach (var image in images)
                {
                    image.DOFade(0f, DeathAnimationFadeDuration);
                }
                yield return new WaitForSeconds(DeathAnimationFadeDuration);
                _momImage.enabled = false;
                break;
            }

            case "Kid":
            {
                var images = _kidImage.GetComponentsInChildren<Image>();
                foreach (var image in images)
                {
                    image.DOFade(0f, DeathAnimationFadeDuration);
                }
                yield return new WaitForSeconds(DeathAnimationFadeDuration);
                _kidImage.enabled = false;
                break;
            }

            default:
                break;
        }
    }
    private void OnFeedButtonClick(string characterKey)
    {
        if (
            _campSystem.Characters[characterKey].IsAlive
            && !_campSystem.Characters[characterKey].IsFed
            && _campSystem.CurrentFood >= _campSystem.Characters[characterKey].FoodRequired
            )
        {
            showCharactersFood(characterKey);
            StartCoroutine(showCharacterSpeech(characterKey));
            _campSystem.Characters[characterKey].IsFed = true;
            _campSystem.CurrentFood -= _campSystem.Characters[characterKey].FoodRequired;
        }
    }

    private void showCharactersFood(string characterName)
    {
        switch (characterName)
        {
            case "Dad":
                _dadFoodImage.enabled = true;
                break;

            case "Mom":
                _momFoodImage.enabled = true;
                break;

            case "Kid":
                _kidFoodImage.enabled = true;
                break;

            default:
                break;
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

        _dadFoodImage.enabled = false;
        _momFoodImage.enabled = false;
        _kidFoodImage.enabled = false;
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

    private void GetUiElementsClicked()
    {
        _clickData.position = Mouse.current.position.ReadValue();
        _clickResults.Clear();

        _uiRaycaster.Raycast(_clickData, _clickResults);

        foreach (RaycastResult result in _clickResults)
        {
            GameObject uiElement = result.gameObject;

            switch (uiElement.name)
            {
                case "DadImage":
                    OnFeedButtonClick("Dad");
                    break;

                case "MomImage":
                    OnFeedButtonClick("Mom");
                    break;

                case "KidImage":
                    OnFeedButtonClick("Kid");
                    break;
                default:
                    break;
            }

            Debug.Log(uiElement.name);
        }
    }

    private void checkEndDayButton()
    {
        _endDayButton.gameObject.SetActive(сheckFedCharacters());
    }

}
