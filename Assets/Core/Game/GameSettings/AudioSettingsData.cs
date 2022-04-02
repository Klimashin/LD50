using System;


[Serializable]
public class AudioSettingsData 
{
	public const float MAX_VOLUME = 1f;
	public const float MIN_VOLUME = 0f;

	public bool isSFXEnabled;
	public bool isMusicEnabled;
	public float volumeSFX;
	public float volumeMusic;

	public AudioSettingsData() 
	{
		isSFXEnabled = true;
		isMusicEnabled = true;
		volumeSFX = MAX_VOLUME;
		volumeMusic = MAX_VOLUME;
	}

	public AudioSettingsData(IAudioSettings audioSettings) 
	{
		isSFXEnabled = audioSettings.isSFXEnabled;
		isMusicEnabled = audioSettings.isMusicEnabled;
		volumeSFX = audioSettings.volumeSFX;
		volumeMusic = audioSettings.volumeMusic;
	}

}