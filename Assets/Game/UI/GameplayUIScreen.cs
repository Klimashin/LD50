using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameplayUIScreen : UIScreen
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private CampSystem _campSystem;
    [SerializeField] private TextMeshProUGUI _foodText;
    [SerializeField] private DayProgressBar _progressBar;
    [SerializeField] private GameObject _foodFlyingIconPrefab;

    public void AddFoodAnimated(int amount)
    {
        var flyingIcon = Instantiate(_foodFlyingIconPrefab, transform);
        flyingIcon.GetComponentInChildren<TextMeshProUGUI>().text = $"+{amount.ToString()}";
        flyingIcon.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        flyingIcon.GetComponent<RectTransform>().DOMove(_foodText.transform.position, 1f)
            .OnComplete(() =>
            {
                _campSystem.CurrentFood += amount;
                Destroy(flyingIcon);
            });
    }

    private void OnEnable()
    {
        _pauseButton.onClick.AddListener(OnPauseButtonClick);
        Game.InputActions.Gameplay.Pause.performed += OnPauseAction;
        Game.InputActions.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _pauseButton.onClick.RemoveListener(OnPauseButtonClick);
        Game.InputActions.Gameplay.Pause.performed -= OnPauseAction;
        Game.InputActions.Gameplay.Disable();
    }

    private void Update()
    {
        _foodText.text = $"Food: {_campSystem.CurrentFood.ToString()}";
    }

    private void OnPauseAction(InputAction.CallbackContext obj)
    {
        if (uiController.GetUIElement<PauseMenuPopup>().isActive)
        {
            uiController.GetUIElement<PauseMenuPopup>().Hide();
        }
        else
        {
            uiController.GetUIElement<PauseMenuPopup>().Show();
        }
    }

    private void OnPauseButtonClick()
    {
        uiController.GetUIElement<PauseMenuPopup>().Show();
    }
}
