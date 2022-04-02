using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameplayUIScreen : UIScreen
{
    [SerializeField] private Button _pauseButton;

    private UIPopup _pauseMenu;

    public override void OnCreate()
    {
        base.OnCreate();
        _pauseMenu = uiController.GetUIElement<PauseMenuPopup>();
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

    private void OnPauseAction(InputAction.CallbackContext obj)
    {
        if (_pauseMenu.isActive)
        {
            _pauseMenu.Hide();
        }
        else
        {
            _pauseMenu.Show();
        }
    }

    private void OnPauseButtonClick()
    {
        _pauseMenu.Show();
    }
}
