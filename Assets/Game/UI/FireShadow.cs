using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FireShadow : MonoBehaviour
{
    [SerializeField] private Image ShadowImage;
    [SerializeField] private Color ShadowColor;
    
    private void OnEnable()
    {
        ShadowImage.DOColor(ShadowColor, 1f).SetLoops(-1, LoopType.Yoyo);
    }
}
