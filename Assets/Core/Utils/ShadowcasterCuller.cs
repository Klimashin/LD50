using System;
using UnityEngine;


public class ShadowcasterCuller : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.ShadowCaster2D _shadowcaster;

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
        if (TryGetComponent<UnityEngine.Rendering.Universal.ShadowCaster2D>(out var shadowcaster))
        {
            Init(shadowcaster);
        }
    }

    public void Init(UnityEngine.Rendering.Universal.ShadowCaster2D shadowcaster)
    {
        _shadowcaster = shadowcaster;
        _isShadowcasterActive = _shadowcaster.castsShadows;

        foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>(true))
        {
            renderer.gameObject.AddComponent<VisibleEventCacher>().Initialize(this);
        }
    }

    private void Update()
    {
        if (
            _isShadowcasterActive && _visibleRendererCount > 0
            || !_isShadowcasterActive && _visibleRendererCount == 0
        )
        {
            return;
        }
        
        if (_isShadowcasterActive && _visibleRendererCount == 0)
        {
            _shadowcaster.castsShadows = false;
            _isShadowcasterActive = false;
        }
        else
        {
            _shadowcaster.castsShadows = true;
            _isShadowcasterActive = true;
        }
    }
}
