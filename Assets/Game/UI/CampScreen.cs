using System;
using UnityEngine;
using UnityEngine.UI;

public class CampScreen : UIScreen
{
    [SerializeField] private CampSystem _campSystem;
    [SerializeField] private Button _endDayButton;

    public Action OnAllCharactersDead;
    public Action OnDayEnded;

    private void OnEnable()
    {
        _endDayButton.onClick.AddListener(OnEndDayButtonClick);
    }
    
    private void OnDisable()
    {
        _endDayButton.onClick.RemoveListener(OnEndDayButtonClick);
    }

    private void OnEndDayButtonClick()
    {
        Hide();
        uiController.ShowUIElement<GameplayUIScreen>();
    }
}
