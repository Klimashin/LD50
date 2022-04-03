using System.Collections;
using UnityEngine;

public class GameplayTest : MonoBehaviour
{
    public CharController charController;
    public LevelGenerator Generator;
    
    private void Start()
    {
        StartCoroutine(GameplayInitializer());
    }

    private IEnumerator GameplayInitializer()
    {
        yield return new WaitForSeconds(0.1f);

        yield return Generator.Generate();
        
        charController.Enable();
    }
}
