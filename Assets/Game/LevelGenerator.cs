using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    private readonly LevelGeneratorSettings _settings;

    private WorldData _worldData;
    
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
        
        _worldData = new WorldData()
        {
            WorldSeed = seed,
            CurrentDay = 0,
            WorldObjectsData = new List<WorldObjectData>()
        };
        
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
                _worldData.WorldObjectsData.Add(new WorldObjectData()
                {
                    WorldPos = levelObject.transform.position,
                    WorldRotation = levelObject.transform.rotation,
                    PrefabAssetAddress = prefab
                });
                WorldGenerationProgress = i / (float)_settings.GenerationCount;
            }
        }
        
        foreach (var col in collidersToDestroy)
        {
            Object.Destroy(col);
        }
        
        Game.FileStorage.Set("worldData", _worldData);
        Game.FileStorage.Save();

        WorldGenerationProgress = 1f;

        yield return null;
    }
}
