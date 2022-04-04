using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public float GenerationSize = 40f;
    public int GenerationCount = 200;
    public List<GameObject> GenerationPrefabs;
    public LayerMask GenerationLayerMask;

    public float WorldGenerationProgress { get; private set; }

    public IEnumerator Generate()
    {
        yield return null;

        var collidersToDestroy = new List<Collider2D>();
        
        for(var i=0; i < GenerationCount; i++)
        {
            var prefab = GenerationPrefabs[Random.Range(0, GenerationPrefabs.Count)];
            var levelObject = Instantiate(prefab);
            levelObject.transform.position = new Vector3(Random.Range(-1f, 1f) * GenerationSize, Random.Range(-1f, 1f) * GenerationSize, 0f);;
            levelObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(0f, 360f)));
            
            yield return null;

            var obstaclesCastCollider = levelObject.GetComponent<Collider2D>();
            var result = new List<Collider2D>();
            var filter = new ContactFilter2D { useLayerMask = true, layerMask = GenerationLayerMask };
            var castResultsCount = obstaclesCastCollider.OverlapCollider(filter, result);
            if (castResultsCount > 0)
            {
                Destroy(levelObject.gameObject);
                i--;
            }
            else
            {
                collidersToDestroy.Add(obstaclesCastCollider);
                WorldGenerationProgress = i / (float)GenerationCount;
            }
        }
        
        foreach (var col in collidersToDestroy)
        {
            Destroy(col);
        }

        WorldGenerationProgress = 1f;
        
        Destroy(GetComponent<Collider2D>());
        
        yield return null;
    }
}
