using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneManager : ISceneManager
{
    private const string CONFIG_FOLDER = "SceneConfigs";

    public event SceneManagerHandler OnSceneLoadStartedEvent;
    public event SceneManagerHandler OnSceneLoadCompletedEvent;

    public Dictionary<string, SceneConfig> ScenesConfigMap { get; }
    public IScene SceneActual { get; private set; }
    public bool isLoading { get; private set; }

    public SceneManager() 
    {
        ScenesConfigMap = new Dictionary<string, SceneConfig>();
        InitializeSceneConfigs();
    }

    private void InitializeSceneConfigs() 
    {
        var allSceneConfigs = Resources.LoadAll<SceneConfig>(CONFIG_FOLDER);
        foreach (var sceneConfig in allSceneConfigs) 
            ScenesConfigMap[sceneConfig.sceneName] = sceneConfig;
    }

    
    
    public Coroutine LoadScene(string sceneName, UnityAction<SceneConfig> sceneLoadedCallback = null) 
    {
        return LoadAndInitializeScene(sceneName, sceneLoadedCallback, true);
    }
    
    public Coroutine InitializeCurrentScene(UnityAction<SceneConfig> sceneLoadedCallback = null) 
    {
        var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        return LoadAndInitializeScene(sceneName, sceneLoadedCallback, false);
    }

    
    protected Coroutine LoadAndInitializeScene(string sceneName, UnityAction<SceneConfig> sceneLoadedCallback, bool loadNewScene) 
    {
        ScenesConfigMap.TryGetValue(sceneName, out var config);

        if (config == null)
        {
            throw new NullReferenceException($"There is no scene ({sceneName}) in the scenes list. The name is wrong or you forget to add it o the list.");
        }

        return Coroutines.StartRoutine(LoadSceneRoutine(config, sceneLoadedCallback, loadNewScene));
    }


    protected virtual IEnumerator LoadSceneRoutine(SceneConfig config, UnityAction<SceneConfig> sceneLoadedCallback, bool loadNewScene = true) 
    {
        LoadingScreen.Instance.Show(this);
            
        isLoading = true;
        OnSceneLoadStartedEvent?.Invoke(config);

        if (loadNewScene)
        {
            yield return Coroutines.StartRoutine(LoadSceneAsyncRoutine(config));
        }

        yield return Coroutines.StartRoutine(InitializeSceneRoutine(config, sceneLoadedCallback));

        yield return new WaitForSecondsRealtime(1f);
        isLoading = false;
        OnSceneLoadCompletedEvent?.Invoke(config);
        sceneLoadedCallback?.Invoke(config);
        
        LoadingScreen.Instance.Hide(this);
    }

    protected IEnumerator LoadSceneAsyncRoutine(SceneConfig config) 
    {
        var asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(config.sceneName);
        asyncOperation.allowSceneActivation = false;

        var progressDivider = 0.9f;
        var progress = asyncOperation.progress / progressDivider;
        
        while (progress < 1f) 
        {
            yield return null;
            progress = asyncOperation.progress / progressDivider;
        }

        asyncOperation.allowSceneActivation = true;
    }

    protected virtual IEnumerator InitializeSceneRoutine(SceneConfig config, UnityAction<SceneConfig> sceneLoadedCallback) 
    {

        SceneActual = new Scene(config);
        yield return null;

        SceneActual.BuildUI();
        yield return null;

        SceneActual.SendMessageOnCreate();
        yield return null;
        
        yield return SceneActual.InitializeAsync();

        SceneActual.Start();
    }

}