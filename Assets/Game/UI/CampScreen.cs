using System;
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

    public Action OnAllCharactersDead;
    public Action OnDayEnded;

    private void OnEnable()
    {
        setCharactersUnfed();
        updateCurrentDayText();
        _endDayButton.onClick.AddListener(OnEndDayButtonClick);
        _dadFeedButton.onClick.AddListener(delegate { OnFeedButtonClick(_dadFeedButton, "Dad"); });
        _momFeedButton.onClick.AddListener(delegate { OnFeedButtonClick(_momFeedButton, "Mom"); });
        _kidFeedButton.onClick.AddListener(delegate { OnFeedButtonClick(_kidFeedButton, "Kid"); });
        OnAllCharactersDead += endDay;
        OnDayEnded += endGame;
    }

    private void OnDisable()
    {
        _endDayButton.onClick.RemoveListener(OnEndDayButtonClick);
        _dadFeedButton.onClick.RemoveListener(delegate { OnFeedButtonClick(_dadFeedButton, "Dad"); });
        _momFeedButton.onClick.RemoveListener(delegate { OnFeedButtonClick(_momFeedButton, "Mom"); });
        _kidFeedButton.onClick.RemoveListener(delegate { OnFeedButtonClick(_kidFeedButton, "Kid"); });
        OnAllCharactersDead -= endDay;
        OnDayEnded += endGame;
    }

    private void Update()
    {
        updateCurrentFoodText();
    }

    private void OnEndDayButtonClick()
    {
        if (сheckFedCharacters())
        {
            if (hasAliveCharacters())
            {
                OnDayEnded();
            } else {
                OnAllCharactersDead();
            }

            dayCounterInc();
            Hide();
            uiController.ShowUIElement<GameplayUIScreen>();
        }
    }

    private void OnFeedButtonClick(Button button, string characterKey)
    {
        if (
            _campSystem.Characters[characterKey].IsAlive
            && !_campSystem.Characters[characterKey].IsFed
            && _campSystem.CurrentFood >= _campSystem.Characters[characterKey].FoodRequired
            )
        {
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
            if (character.Value.IsAlive && !character.Value.IsFed)
            {
                if (_campSystem.CurrentFood >= character.Value.FoodRequired)
                {
                    return false;
                }
                character.Value.IsAlive = false;
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

    private void endDay()
    {
        Debug.Log("endDay");
    }

    private void endGame()
    {
        Debug.Log("endGame");
    }

}
