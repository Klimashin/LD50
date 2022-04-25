using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampScreen : UIScreen
{
    [SerializeField] private CampSystem _campSystem;
    [SerializeField] private Button _endDayButton;
    [SerializeField] private TextMeshProUGUI _foodText;
    [SerializeField] private TextMeshProUGUI _currentDayText;
    [SerializeField] private SoundSystem _soundSystem;
    [SerializeField] private AudioClip _deathbellAudio;
    [SerializeField] private List<CharacterUI> _characterUis;
    
    private float _audioTimeCached;
    private AudioSource _campAudio;
    private readonly Dictionary<string, CharacterUI> _characterNameToUiDict = new ();

    public override void OnCreate()
    {
        base.OnCreate();
        
        foreach (var characterUi in _characterUis)
        {
            _characterNameToUiDict[characterUi.Name] = characterUi;
        }
        
        _endDayButton.onClick.AddListener(OnEndDayButtonClick);
        _campAudio = GetComponent<AudioSource>();
    }

    protected override void OnPreHide()
    {
        _audioTimeCached = _campAudio.time;
        _campAudio.Pause();
    }

    protected override void OnPostShow()
    {
        base.OnPostShow();

        var worldData = Game.FileStorage.Get<WorldData>("worldData");
        worldData.CurrentDay = _campSystem.CurrentDay;
        worldData.CurrentFood = _campSystem.CurrentFood;
        Game.FileStorage.Save();
        
        _campAudio.volume = 0f;
        _campAudio.time = _audioTimeCached;
        _campAudio.Play();
        _campAudio.DOFade(1f, 1f);
    }

    private void OnEnable()
    {
        SetCharactersUnfed();
        UpdateCurrentDayText();
    }

    private void Update()
    {
        UpdateCurrentFoodText();
        _endDayButton.gameObject.SetActive(!SomeCharacterCanBeFed());
    }

    private void OnEndDayButtonClick()
    {
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
                Game.FileStorage.Get<WorldData>("worldData").DeadCharacters.Add(characterName);
                _soundSystem.PlayOneShot(_deathbellAudio);
                yield return _characterNameToUiDict[characterName].DeathAnimation();
            }

            yield return new WaitForSeconds(2f);
        }

        if (HasAliveCharacters())
        {
            _campSystem.CurrentDay++;
            Hide();
            UIController.ShowUIElement<GameplayUIScreen>();
        }
        else
        {
            Hide();
            UIController.ShowUIElement<EndGameScreen>();
        }
    }

    private void SetCharactersUnfed()
    {
        foreach (var (charName, charData) in _campSystem.Characters)
        {
            if (_characterNameToUiDict.ContainsKey(charName))
                _characterNameToUiDict[charName].SetUnfed();

            if (charData.IsAlive)
                charData.IsFed = false;
        }
    }

    private void UpdateCurrentDayText()
    {
        _currentDayText.text = $"Day: {_campSystem.CurrentDay.ToString()}";
    }

    private bool SomeCharacterCanBeFed()
    {
        return _campSystem.Characters.Values.ToList()
            .Exists(character => character.IsAlive && !character.IsFed && _campSystem.CurrentFood >= character.FoodRequired);
    }

    private bool HasAliveCharacters()
    {
        return _campSystem.Characters.Values.ToList()
            .Exists(character => character.IsAlive);
    }

    private void UpdateCurrentFoodText()
    {
        _foodText.text = $"X {_campSystem.CurrentFood.ToString()}";
    }

    private List<string> KillStarvingCharacters()
    {
        var result = new List<string>();
        foreach (var character in _campSystem.Characters.Values.Where(character => character.IsAlive && !character.IsFed))
        {
            character.IsAlive = false;
            result.Add(character.Name);
        }

        return result;
    }
}
