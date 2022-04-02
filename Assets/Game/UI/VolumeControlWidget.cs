using UnityEngine;
using UnityEngine.UI;

public class VolumeControlWidget : UIWidget
{
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private SoundSystem _soundSystem;

    private void OnEnable()
    {
        _volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDisable()
    {
        _volumeSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float v)
    {
        _soundSystem.SetVolume(v);
    }
}
