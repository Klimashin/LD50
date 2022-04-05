
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class FireAnimator : MonoBehaviour
{
    public Sprite[] Sprites;
    public float SwitchTimeout;
    public Image ShadowImage;
    public Color ShadowColor;

    private int _index = 0;
    private float _timePassed;
    private Image _fireImage;


    private TweenerCore<Color, Color, ColorOptions> _shadowTween;
    private void OnEnable()
    {
        _fireImage = GetComponent<Image>();
        ShadowImage.DOColor(ShadowColor, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        if (_shadowTween != null && _shadowTween.IsActive())
        {
            _shadowTween.Kill();
            _shadowTween = null;
        }
    }

    private void Update()
    {
        _timePassed += Time.deltaTime;
        if (_timePassed >= SwitchTimeout)
        {
            _timePassed = 0;
            _index++;
            if (_index >= Sprites.Length)
            {
                _index = 0;
            }

            _fireImage.sprite = Sprites[_index];
        }
    }
}
