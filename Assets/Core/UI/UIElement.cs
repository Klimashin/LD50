using System;
using UnityEngine;

public abstract class UIElement : MonoBehaviour, IUIElement 
{
	public event Action<IUIElement> OnElementHideStartedEvent;
	public event Action<IUIElement> OnElementHiddenCompletelyEvent;
	public event Action<IUIElement> OnElementShownEvent;
	public event Action<IUIElement> OnElementDestroyedEvent;

	public bool isActive { get; protected set; } = true;
	protected UIController UIController => UI.controller;

	public virtual void Show() {
		if (isActive)
		{
			return;
		}

		OnPreShow();
		gameObject.SetActive(true);
		isActive = true;
		OnPostShow();
		NotifyAboutShown();
	}

	protected virtual void OnPreShow() { }
	protected virtual void OnPostShow() { }

	protected void NotifyAboutShown() 
	{
		OnElementShownEvent?.Invoke(this);
	}

	public virtual void Hide() 
	{
		if (!isActive)
		{
			return;
		}

		NotifyAboutHideStarted();

		HideInstantly();
	}

	protected void NotifyAboutHideStarted() 
	{
		OnPreHide();
		OnElementHideStartedEvent?.Invoke(this);
	}

	public virtual void HideInstantly() {
		if (!isActive)
		{
			return;
		}

		isActive = false;
		gameObject.SetActive(false);
		OnPostHide();
		OnElementHiddenCompletelyEvent?.Invoke(this);
	}

	protected virtual void OnPreHide() { }
	protected virtual void OnPostHide() { }
	
	protected virtual void Handle_AnimationOutOver()
	{
		HideInstantly();
	}
}
