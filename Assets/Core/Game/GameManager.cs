using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnApplicationPausedEvent;
    public static event Action OnApplicationUnpausedEvent;
    public static event Action OnApplicationFocusedEvent;
    public static event Action OnApplicationUnfocusedEvent;
    public static event Action OnApplicationQuitEvent;

    [SerializeField] private bool saveOnPause;
    [SerializeField] private bool saveOnUnfocus = true;
    [SerializeField] private bool saveOnExit = true;
    [Space, SerializeField] private bool isLoggingEnabled;

    private void Start() 
    {
        DontDestroyOnLoad(gameObject);
        
        Game.Run();

        if (isLoggingEnabled)
        {
            Debug.Log($"GAME MANAGER: Game launched: {Application.productName}");
        }
    }

    
    private void OnApplicationPause(bool pauseStatus) 
    {
        if (pauseStatus) {
            if (isLoggingEnabled)
                Debug.Log("GAME MANAGER: Paused");
            
            if (saveOnPause)
                Game.SaveGame();
            
            OnApplicationPausedEvent?.Invoke();
        }
        else {
            if (isLoggingEnabled)
                Debug.Log("GAME MANAGER: Game unpaused");
            
            OnApplicationUnpausedEvent?.Invoke();
        }
    }


    private void OnApplicationFocus(bool hasFocus) {
        if (!hasFocus) {
            if (isLoggingEnabled)
                Debug.Log("GAME MANAGER: Game focused");
            
            if (saveOnUnfocus)
                Game.SaveGame();
            
            OnApplicationUnfocusedEvent?.Invoke();
        }
        else {
            if (isLoggingEnabled)
                Debug.Log("GAME MANAGER: Game unfocused");
            
            OnApplicationFocusedEvent?.Invoke();
        }
    }
    

    private void OnApplicationQuit() {
        if (isLoggingEnabled)
            Debug.Log("GAME MANAGER: Game exited");
        
        if (saveOnExit)
            Game.SaveGame();
        
        OnApplicationQuitEvent?.Invoke();
    }

}