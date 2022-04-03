using System.Collections;
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
    [SerializeField] private float _gameplayTime;

    private CharController _charController;

    public override void OnCreate()
    {
        _charController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>();
    }

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

    private void SetDayProgress(float progress)
    {
        _progressBar.SetProgress(progress);
    }

    protected override void OnPostShow()
    {
        base.OnPostShow();
        _pauseButton.onClick.AddListener(OnPauseButtonClick);
        Game.InputActions.Gameplay.Pause.performed += OnPauseAction;
        Game.InputActions.Gameplay.Enable();

        StartCoroutine(GameplayCoroutine());
    }

    protected override void OnPreHide()
    {
        base.OnPreHide();
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
        var pauseMenu = uiController.GetUIElement<PauseMenuPopup>();
        if (pauseMenu.isActive)
        {
            pauseMenu.Hide();
        }
        else
        {
            pauseMenu.Show();
        }
    }

    private void OnPauseButtonClick()
    {
        uiController.GetUIElement<PauseMenuPopup>().Show();
    }
    
    private IEnumerator GameplayCoroutine()
    {
        _charController.Enable();
        
        var currentTime = 0f;
        while (currentTime <= _gameplayTime)
        {
            currentTime += Time.deltaTime;
            var progress = currentTime / _gameplayTime;
            SetDayProgress(progress);
            yield return null;
        }
        
        _charController.Disable();

        Hide();
        uiController.GetUIElement<CampScreen>().Show();
    }
}
