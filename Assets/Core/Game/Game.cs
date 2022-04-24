using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Game 
{
    public static event Action OnGameInitializedEvent;

    public static ArchitectureComponentState state { get; private set; } = ArchitectureComponentState.NotInitialized;
    public static bool isInitialized => state == ArchitectureComponentState.Initialized;
    public static ISceneManager SceneManager { get; private set; }
    public static InputActions InputActions { get; private set; }
    public static FileStorage FileStorage { get; private set; }

    #region GAME RUNNING

    public static void Run() 
    {
        Coroutines.StartRoutine(RunGameRoutine());
    }

    private static IEnumerator RunGameRoutine() 
    {
        state = ArchitectureComponentState.Initializing;
        InputActions = new InputActions();
        
        FileStorage = new FileStorage();
        FileStorage.Load();
        
        yield return null;
        
        yield return InitSceneManager();

        state = ArchitectureComponentState.Initialized;
        OnGameInitializedEvent?.Invoke();
    }

    private static IEnumerator InitSceneManager() 
    {
        SceneManager = new SceneManager();
        yield return SceneManager.InitializeCurrentScene();
    }

    #endregion

    public static T GetInteractor<T>() where T : IInteractor 
    {
        return SceneManager.CurrentScene.GetInteractor<T>();
    }

    public static IEnumerable<T> GetInteractors<T>() where T : IInteractor 
    {
        return SceneManager.CurrentScene.GetInteractors<T>();
    }

    public static T GetRepository<T>() where T : IRepository 
    {
        return SceneManager.CurrentScene.GetRepository<T>();
    }
    
    public static IEnumerable<T> GetRepositories<T>() where T : IRepository 
    {
        return SceneManager.CurrentScene.GetRepositories<T>();
    }

    public static void SaveWorldData(WorldData worldData) 
    {
        FileStorage.Save();
    }

    public static void SaveGameAsync(Action callback) 
    {
        FileStorage.SaveAsync(callback);
    }

    public static IEnumerator SaveWithRoutine(Action callback) 
    {
        yield return FileStorage.SaveWithRoutine();
    }
}