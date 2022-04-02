using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundSystem", menuName = "Systems/SoundSystem")]
public class SoundSystem : ScriptableObject
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string VolumeVariableName;

    public void SetVolume(float v)
    {
        var volumeLvl = v > float.Epsilon ? 20 * Mathf.Log10(v) : -144f;
        _audioMixer.SetFloat(VolumeVariableName, volumeLvl);
    }
}
