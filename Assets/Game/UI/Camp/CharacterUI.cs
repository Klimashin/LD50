using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CharacterUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string Name;

    [SerializeField] private CampSystem _campSystemRef;
    [SerializeField] public RectTransform _speechArea;
    [SerializeField] public TextMeshProUGUI _speechText; 
    [SerializeField] public Image _characterImage;
    [SerializeField] public Image _characterFoodImage;
    [SerializeField] public Image _characterOutlineImage;

    private CharacterData _characterData;
    private void Awake()
    {
        _characterData = _campSystemRef.Characters[Name];
    }

    private void Update()
    {
        if (!CanBeFed())
        {
            _characterOutlineImage.gameObject.SetActive(false);
        }
    }
    
    private const float DEATH_ANIMATION_FADE_DURATION = 2f;
    public IEnumerator DeathAnimation()
    {
        var images = _characterImage.GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            image.DOFade(0f, DEATH_ANIMATION_FADE_DURATION);
        }
        yield return new WaitForSeconds(DEATH_ANIMATION_FADE_DURATION);

        gameObject.SetActive(false);
    }

    private IEnumerator FeedAnimation()
    {
        yield return null;
        _characterFoodImage.enabled = true;
    }

    public void SetUnfed()
    {
        _characterFoodImage.enabled = false;
    }
    
    private const float REPLICA_SHOW_DURATION = 5f;
    private IEnumerator ShowSpeechBubble()
    {
        var replicas = _characterData.SpeechArray;
        var replica = replicas[Random.Range(0, replicas.Length)];
        _speechText.text = replica;

        _speechArea.gameObject.SetActive(true);

        yield return new WaitForSeconds(REPLICA_SHOW_DURATION);

        _speechArea.gameObject.SetActive(false);
    }

    private bool CanBeFed()
    {
        return _characterData.IsAlive && !_characterData.IsFed &&
               _campSystemRef.CurrentFood >= _characterData.FoodRequired;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CanBeFed())
        {
            _characterOutlineImage.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _characterOutlineImage.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!CanBeFed())
        {
            return;
        }
        
        StartCoroutine(FeedAnimation());
        StartCoroutine(ShowSpeechBubble());
        _characterData.IsFed = true;
        _campSystemRef.CurrentFood -= _characterData.FoodRequired;
    }

    private void OnDisable()
    {
        _speechArea.gameObject.SetActive(false);
    }
}
