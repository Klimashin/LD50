using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameplayUIScreen : UIScreen
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private CampSystem _campSystem;
    [SerializeField] private TextMeshProUGUI _foodText;

    //private UIPopup _pauseMenu;

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
