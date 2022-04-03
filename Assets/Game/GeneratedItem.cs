using UnityEngine;
using Random = UnityEngine.Random;

public abstract class GeneratedItem : MonoBehaviour
{
    [Range(0, 100)]
    public int GenerationChance = 100;

    private void Awake()
    {
        GenerationCheck();
    }

    public void GenerationCheck()
    {
        var roll = Random.Range(0, 101);
        if (roll > GenerationChance)
        {
            Destroy(gameObject);
        }
    }
}
