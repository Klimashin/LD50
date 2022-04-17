
using UnityEngine;
using UnityEngine.UI;

public class FireAnimator : MonoBehaviour
{
    public Sprite[] Sprites;
    public float SwitchTimeout;

    private int _index = 0;
    private float _timePassed;
    private Image _fireImage;
    
    private void OnEnable()
    {
        _fireImage = GetComponent<Image>();
    }

    private void Update()
    {
        _timePassed += Time.deltaTime;
        if (!(_timePassed >= SwitchTimeout))
        {
            return;
        }

        _timePassed = 0;
        _index++;
        if (_index >= Sprites.Length)
        {
            _index = 0;
        }

        _fireImage.sprite = Sprites[_index];
    }
}
