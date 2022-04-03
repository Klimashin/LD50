using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ShadowcasterCuller : MonoBehaviour
{
    private Behaviour _shadowcaster;

    private int _count = 0;

    private bool _isShadowcasterActive;

    private int _visibleRendererCount { get => _count; set => _count = Math.Max(0, value); }

    private class VisibleEventCacher : MonoBehaviour
    {
        public ShadowcasterCuller _parent;

        void OnBecameInvisible()
        {
            _parent._visibleRendererCount--;
        }

        void OnBecameVisible()
        {
            _parent._visibleRendererCount++;
        }

        internal void Initialize(ShadowcasterCuller parent)
        {
            _parent = parent;

            if (GetComponent<Renderer>().isVisible)
                OnBecameVisible();
        }
    }

    private void Awake()
    {
        if (TryGetComponent<ShadowCaster2D>(out var shadowcaster))
        {
            Init(shadowcaster);
        }
    }

    public void Init(Behaviour shadowcaster)
    {
        _shadowcaster = shadowcaster;
        _isShadowcasterActive = _shadowcaster.enabled;

        foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>(true))
        {
            renderer.gameObject.AddComponent<VisibleEventCacher>().Initialize(this);
        }
    }

    private void Update()
    {
        if (_isShadowcasterActive != (_visibleRendererCount > 0))
            _shadowcaster.enabled = _isShadowcasterActive = (_visibleRendererCount > 0);
    }
}
