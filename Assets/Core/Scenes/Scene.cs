using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public sealed class Scene : IScene 
{
    public SceneConfig sceneConfig { get; }
    public ComponentsBase<IRepository> repositoriesBase { get; }
    public ComponentsBase<IInteractor> interactorsBase { get; }
    
    public Storage fileStorage { get; private set; }


    public Scene(SceneConfig config) 
    {
        sceneConfig = config;
        repositoriesBase = new ComponentsBase<IRepository>(config.RepositoriesReferences);
        interactorsBase = new ComponentsBase<IInteractor>(config.InteractorsReferences);
    }

    public void BuildUI()
    {
        UI.Build(sceneConfig);
    }
    
    public void SendMessageOnCreate() 
    {
        repositoriesBase.SendMessageOnCreate();
        repositoriesBase.SendMessageOnCreate();
        UI.controller.SendMessageOnCreate();
    }


    #region INITIALIZE

    public Coroutine InitializeAsync() 
    {
        return Coroutines.StartRoutine(InitializeAsyncRoutine());
    }

    private IEnumerator InitializeAsyncRoutine() 
    {
        // TODO: Load storage here if needed.
        if (sceneConfig.SaveDataForThisScene) {
            fileStorage = new FileStorage(sceneConfig.SaveName);
            fileStorage.Load();
        }

        yield return repositoriesBase.InitializeAllComponents();
        yield return interactorsBase.InitializeAllComponents();
        
        repositoriesBase.SendMessageOnInitialize();
        interactorsBase.SendMessageOnInitialize();
        UI.controller.SendMessageOnInitialize();
    }

    #endregion

    public void Start() 
    {
        repositoriesBase.SendMessageOnStart();
        interactorsBase.SendMessageOnStart();
        UI.controller.SendMessageOnStart();
    }

    public void Save() 
    {
        fileStorage?.Save();
    }

    public T GetRepository<T>() where T : IRepository 
    {
        return repositoriesBase.GetComponent<T>();
    }

    public IEnumerable<T> GetRepositories<T>() where T : IRepository 
    {
        return repositoriesBase.GetComponents<T>();
    }

    public T GetInteractor<T>() where T : IInteractor 
    {
        return interactorsBase.GetComponent<T>();
    }
    
    public IEnumerable<T> GetInteractors<T>() where T : IInteractor 
    {
        return interactorsBase.GetComponents<T>();
    }

}