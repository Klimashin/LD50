using System;


public interface IVibroSettings : ISettings 
{
	event Action OnVibroStateChangedEvent;

	bool isEnabled { get; set; }
}