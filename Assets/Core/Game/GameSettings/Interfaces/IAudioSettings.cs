using System;


public interface IAudioSettings : ISettings 
{
	event Action OnVolumeSFXChangedEvent;
	event Action OnVolumeMusicChangedEvent;

	bool isEnabled { get; set; }
	bool isSFXEnabled { get; set; }
	bool isMusicEnabled { get; set; }
	float volumeSFX { get; set; }
	float volumeMusic { get; set; }
}