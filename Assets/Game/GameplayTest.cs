using System.Collections;
using UnityEngine;

public class GameplayTest : MonoBehaviour
{
    private void OnEnable()
    {
        Game.SceneManager.OnSceneLoadCompletedEvent += SceneManagerOnOnSceneLoadCompletedEvent;
    }

    private void OnDisable()
    {
        Game.SceneManager.OnSceneLoadCompletedEvent -= SceneManagerOnOnSceneLoadCompletedEvent;
    }

    private void SceneManagerOnOnSceneLoadCompletedEvent(SceneConfig config)
    {
        StartCoroutine(GameplayInitializer());
    }

    private IEnumerator GameplayInitializer()
    {
        yield return new WaitForSeconds(1f);
        
        Debug.Log("HERE");
    }
}
