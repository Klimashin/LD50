using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void SceneManagerHandler(SceneConfig config);
    
public interface ISceneManager 
{
    event SceneManagerHandler OnSceneLoadStartedEvent;
    event SceneManagerHandler OnSceneLoadCompletedEvent;

    IScene SceneActual { get; }
    Dictionary<string, SceneConfig> ScenesConfigMap { get; }

    Coroutine LoadScene(string sceneName, UnityAction<SceneConfig> sceneLoadedCallback = null);
    Coroutine InitializeCurrentScene(UnityAction<SceneConfig> sceneLoadedCallback = null);
}