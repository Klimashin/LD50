using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    private readonly LevelGeneratorSettings _settings;
    
    public LevelGenerator(LevelGeneratorSettings settings)
    {
        _settings = settings;
    }

    public float WorldGenerationProgress { get; private set; }

    public Coroutine Generate(int seed)
    {
        return Coroutines.StartRoutine(GenerationRoutine(seed));
    }

    private IEnumerator GenerationRoutine(int seed)
    {
        Random.InitState(seed);
        
        yield return null;

        var collidersToDestroy = new List<Collider2D>();
        
        for(var i=0; i < _settings.GenerationCount; i++)
        {
            var prefab = _settings.GenerationPrefabs[Random.Range(0, _settings.GenerationPrefabs.Count)];
            var levelObjectHandle = prefab.InstantiateAsync();
            while (!levelObjectHandle.IsDone)
            {
                yield return null;
            }

            var levelObject = levelObjectHandle.Result;
            levelObject.transform.position = new Vector3(Random.Range(-1f, 1f) * _settings.GenerationSize, Random.Range(-1f, 1f) * _settings.GenerationSize, 0f);;
            levelObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(0f, 360f)));
            
            yield return null;

            var obstaclesCastCollider = levelObject.GetComponent<Collider2D>();
            var result = new List<Collider2D>();
            var filter = new ContactFilter2D { useLayerMask = true, layerMask = _settings.GenerationLayerMask };
            var castResultsCount = obstaclesCastCollider.OverlapCollider(filter, result);
            if (castResultsCount > 0)
            {
                Object.Destroy(levelObject.gameObject);
                i--;
            }
            else
            {
                collidersToDestroy.Add(obstaclesCastCollider);
                WorldGenerationProgress = i / (float)_settings.GenerationCount;
            }
        }
        
        foreach (var col in collidersToDestroy)
        {
            Object.Destroy(col);
        }

        WorldGenerationProgress = 1f;

        yield return null;
    }
}
