using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Game 
{
    public static event Action OnGameInitializedEvent;

    public static ArchitectureComponentState state { get; private set; } = ArchitectureComponentState.NotInitialized;
    public static bool isInitialized => state == ArchitectureComponentState.Initialized;
    public static ISceneManager SceneManager { get; private set; }
    public static IGameSettings GameSettings { get; private set; }
    public static InputActions InputActions { get; private set; }

    #region GAME RUNNING

    public static void Run() 
    {
        Coroutines.StartRoutine(RunGameRoutine());
    }

    private static IEnumerator RunGameRoutine() 
    {
        state = ArchitectureComponentState.Initializing;
        InputActions = new InputActions();

        InitGameSettings();
        yield return null;
        
        InitSceneManager();
        yield return null;

        yield return SceneManager.InitializeCurrentScene();

        state = ArchitectureComponentState.Initialized;
        OnGameInitializedEvent?.Invoke();
    }
    
    private static void InitGameSettings() 
    {
        GameSettings = new GameSettings();
    }

    private static void InitSceneManager() 
    {
        SceneManager = new SceneManager();
    }

    #endregion

    public static T GetInteractor<T>() where T : IInteractor 
    {
        return SceneManager.SceneActual.GetInteractor<T>();
    }

    public static IEnumerable<T> GetInteractors<T>() where T : IInteractor 
    {
        return SceneManager.SceneActual.GetInteractors<T>();
    }

    public static T GetRepository<T>() where T : IRepository 
    {
        return SceneManager.SceneActual.GetRepository<T>();
    }
    
    public static IEnumerable<T> GetRepositories<T>() where T : IRepository 
    {
        return SceneManager.SceneActual.GetRepositories<T>();
    }

    public static void SaveGame() 
    {
        SceneManager.SceneActual.fileStorage.Save();
    }

    public static void SaveGameAsync(Action callback) 
    {
        SceneManager.SceneActual.fileStorage.SaveAsync(callback);
    }

    public static IEnumerator SaveWithRoutine(Action callback) 
    {
        yield return SceneManager.SceneActual.fileStorage.SaveWithRoutine();
    }
}