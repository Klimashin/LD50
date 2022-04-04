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
    private PauseMenuPopup _pauseMenu;
    private AudioSource _gameplayAudio;

    public override void OnCreate()
    {
        base.OnCreate();
        _gameplayAudio = GetComponent<AudioSource>();
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
        _pauseMenu = uiController.GetUIElement<PauseMenuPopup>();
        _charController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>();

        _pauseMenu.OnElementShownEvent += OnPauseMenuShown;
        _pauseMenu.OnElementHideStartedEvent += OnPauseMenuHide;
        _pauseButton.onClick.AddListener(OnPauseButtonClick);
        Game.InputActions.Gameplay.Pause.performed += OnPauseAction;
        Game.InputActions.Gameplay.Enable();
        Cursor.visible = false;
        _gameplayAudio.volume = 0f;
        _gameplayAudio.Play();
        _gameplayAudio.DOFade(1f, 1f);

        StartCoroutine(GameplayCoroutine());
    }

    protected override void OnPreHide()
    {
        base.OnPreHide();

        _pauseButton.onClick.RemoveListener(OnPauseButtonClick);
        Game.InputActions.Gameplay.Pause.performed -= OnPauseAction;
        _pauseMenu.OnElementShownEvent -= OnPauseMenuShown;
        _pauseMenu.OnElementHideStartedEvent -= OnPauseMenuHide;
        Game.InputActions.Gameplay.Disable();
        Cursor.visible = true;
        _gameplayAudio.Pause();
    }

    private void Update()
    {
        _foodText.text = $"X {_campSystem.CurrentFood.ToString()}";
    }

    private void OnPauseMenuShown(IUIElement menu)
    {
        Time.timeScale = 0;
        _charController.Disable();
        Cursor.visible = true;
    }

    private void OnPauseMenuHide(IUIElement menu)
    {
        Time.timeScale = 1;
        _charController.Enable();
        Cursor.visible = false;
        Debug.Log("HERE");
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
        _charController.ResetPosition();
        
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
