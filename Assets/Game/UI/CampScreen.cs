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


    public Action OnAllCharactersDead;
    public Action OnDayEnded;

    private void OnEnable()
    {
        setCharactersUnfed();
        _endDayButton.onClick.AddListener(OnEndDayButtonClick);
        _dadFeedButton.onClick.AddListener(delegate{OnFeedButtonClick("Dad");});
        _momFeedButton.onClick.AddListener(delegate{OnFeedButtonClick("Mom");});
        _kidFeedButton.onClick.AddListener(delegate{OnFeedButtonClick("Kid");});
    }
    
    private void OnDisable()
    {
        _endDayButton.onClick.RemoveListener(OnEndDayButtonClick);
        _dadFeedButton.onClick.RemoveListener(delegate{OnFeedButtonClick("Dad");});
        _momFeedButton.onClick.RemoveListener(delegate{OnFeedButtonClick("Mom");});
        _kidFeedButton.onClick.RemoveListener(delegate{OnFeedButtonClick("Kid");});
    }

    private void Update()
    {
        сurrentFoodValueUpdate();
    }

    private void OnEndDayButtonClick()
    {
        сurrentFoodValueUpdate();
        сheckFedCharacters();
        dayCounterInc();
        Hide();
        uiController.ShowUIElement<GameplayUIScreen>();
    }

    private void OnFeedButtonClick(string characterKey) 
    {
        if (_campSystem.Characters[characterKey].IsAlive && !_campSystem.Characters[characterKey].IsFed)
        {
             _campSystem.Characters[characterKey].IsFed = true;
             _campSystem.CurrentFood -= _campSystem.Characters[characterKey].FoodRequired;
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

    private void сheckFedCharacters() 
    {

        foreach (var character in _campSystem.Characters)
            {
               if (character.Value.IsAlive && !character.Value.IsFed) 
               {
                   character.Value.IsAlive = false;
               }
            }
    }

    private void dayCounterInc() 
    {
        _campSystem.CurrentDay ++;
    }

    private void сurrentFoodValueUpdate()
    {
        // _campSystem.CurrentFood ++;
        _foodText.text = $"Food: {_campSystem.CurrentFood.ToString()}";
    }

}
