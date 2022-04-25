using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CampScreen : UIScreen
{
    [SerializeField] private CampSystem _campSystem;
    [SerializeField] private Button _endDayButton;
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
        
        _campSystem.StartCampPhase();

        _campAudio.volume = 0f;
        _campAudio.time = _audioTimeCached;
        _campAudio.Play();
        _campAudio.DOFade(1f, 1f);
    }

    private void Update()
    {
        _endDayButton.gameObject.SetActive(!SomeCharacterCanBeFed());
    }

    private void OnEndDayButtonClick()
    {
        StartCoroutine(EndDayCoroutine());
    }

    private IEnumerator EndDayCoroutine()
    {
        yield return null;

        var charactersDiedToday = _campSystem.EndCampPhase();
        if (charactersDiedToday.Count > 0)
        {
            foreach (var characterName in charactersDiedToday)
            {
                _soundSystem.PlayOneShot(_deathbellAudio);
                yield return _characterNameToUiDict[characterName].DeathAnimation();
            }

            yield return new WaitForSeconds(2f);
        }
        
        Hide();
        if (_campSystem.HasAliveCharacters())
        {
            UIController.ShowUIElement<GameplayUIScreen>();
        }
        else
        {
            UIController.ShowUIElement<EndGameScreen>();
        }
    }

    private bool SomeCharacterCanBeFed()
    {
        return _campSystem.Characters.Values.ToList()
            .Exists(character => character.IsAlive && !character.IsFed && _campSystem.CurrentFood >= character.FoodRequired);
    }
}
