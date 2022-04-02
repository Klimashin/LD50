using System.Collections.Generic;
using UnityEngine;

public interface IScene 
{
    
    SceneConfig sceneConfig { get; }
    ComponentsBase<IRepository> repositoriesBase { get; }
    ComponentsBase<IInteractor> interactorsBase { get; }
    Storage fileStorage { get; }


    void BuildUI();
    void SendMessageOnCreate();
    Coroutine InitializeAsync();
    void Start();
    void Save();

    T GetRepository<T>() where T : IRepository;
    IEnumerable<T> GetRepositories<T>() where T : IRepository;
    
    T GetInteractor<T>() where T : IInteractor;
    IEnumerable<T> GetInteractors<T>() where T : IInteractor;
}