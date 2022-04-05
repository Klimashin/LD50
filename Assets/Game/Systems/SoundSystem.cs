using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundSystem", menuName = "Systems/SoundSystem")]
public class SoundSystem : ScriptableObject
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string VolumeVariableName;

    private float Volume { get; set; } = 1;

    public void SetVolume(float v)
    {
        Volume = v;
        var volumeLvl = v > float.Epsilon ? 20 * Mathf.Log10(Volume) : -144f;
        _audioMixer.SetFloat(VolumeVariableName, volumeLvl);
    }

    public float GetVolume()
    {
        return Volume;
    }

    private AudioSource _oneShotAudio;
    public void PlayOneShot(AudioClip clip, float volScale = 1f)
    {
        if (_oneShotAudio == null)
        {
            _oneShotAudio = (new GameObject()).AddComponent<AudioSource>();
            GameObject.DontDestroyOnLoad(_oneShotAudio);
            _oneShotAudio.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Sfx")[0];
        }
        
        _oneShotAudio.PlayOneShot(clip, volScale);
    }
}
