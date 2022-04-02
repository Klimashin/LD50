using System;


public class VibroSettings : IVibroSettings 
{
	private const string KEY_VIBRO_SETTINGS = "VIBRO_SETTINGS";

	public event Action OnVibroStateChangedEvent;

	public bool isEnabled 
	{
		get => vibroData.isVibroEnabled;
		set 
		{
			vibroData.isVibroEnabled = value;
			OnVibroStateChangedEvent?.Invoke();
		}
	}

	private Storage gameSettingsStorage;
	private VibroSettingsData vibroData;

	public VibroSettings(Storage gameSettingsStorage) 
	{
		this.gameSettingsStorage = gameSettingsStorage;
		var vibroDataDefault = new VibroSettingsData();
		var loadedData = gameSettingsStorage.Get(KEY_VIBRO_SETTINGS, vibroDataDefault);
		vibroData = loadedData;
	}

	public void Save() 
	{
		gameSettingsStorage.Set(KEY_VIBRO_SETTINGS, vibroData);
	}

	public override string ToString() 
	{
		var line = $"VIBRO: is enabled = {isEnabled}";
		return line;
	}
}