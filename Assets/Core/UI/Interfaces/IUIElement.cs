﻿using System;
using UnityEngine;

public interface IUIElement 
{
	event Action<IUIElement> OnElementHideStartedEvent;
	event Action<IUIElement> OnElementHiddenCompletelyEvent;
	event Action<IUIElement> OnElementShownEvent;
	event Action<IUIElement> OnElementDestroyedEvent;

	bool isActive { get; }
	string name { get; }
	GameObject gameObject { get; }

	void Show();
	void Hide();
	void HideInstantly();

}