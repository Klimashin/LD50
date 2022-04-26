using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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
        var generationResult = new Dictionary<WorldObject, WorldObjectData>();
        
        Random.InitState(seed);
        
        _worldData = new WorldData()
        {
            WorldSeed = seed,
            WorldObjectsData = new List<WorldObjectData>()
        };
        
        yield return null;

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
                var objectWorldData = new WorldObjectData
                {
                    WorldPos = levelObject.transform.position,
                    WorldRotation = levelObject.transform.rotation,
                    PrefabAssetAddress = prefab,
                    ObjectSeed = Random.Range(1, int.MaxValue)
                };
                
                _worldData.WorldObjectsData.Add(objectWorldData);

                var worldObjectComponent = levelObject.GetComponent<WorldObject>();
                Debug.Assert(worldObjectComponent != null, $"levelObject {levelObject.name} missing WorldObject component!");

                generationResult[worldObjectComponent] = objectWorldData;

                WorldGenerationProgress = i / (float)_settings.GenerationCount;
            }
        }
        
        // this should always be called after all seeded objects are placed
        foreach (var (worldObject, worldObjectData) in generationResult)
        {
            worldObject.Initialize(worldObjectData);
        }

        Game.FileStorage.Set("worldData", _worldData);

        WorldGenerationProgress = 1f;

        yield return null;
    }
}
