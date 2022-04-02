using System.Collections;
using UnityEngine;

public class GameplayTest : MonoBehaviour
{
    public CharController charController;
    
    private void Start()
    {
        StartCoroutine(GameplayInitializer());
    }

    private IEnumerator GameplayInitializer()
    {
        yield return new WaitForSeconds(0.1f);
        
        charController.Enable();
    }
}
