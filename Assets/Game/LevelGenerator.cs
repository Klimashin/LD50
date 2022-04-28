using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class LevelGenerator
{
    private readonly LevelGeneratorSettings _settings;
    private readonly Dictionary<WorldObject, WorldObjectData> _generationResult = new ();

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
            WorldObjectsData = new List<WorldObjectData>()
        };

        yield return GenerationStep();

        yield return InitializationStep();
    }

    private IEnumerator GenerationStep()
    {
        yield return null;

        var totalObjectsToSpawn = _settings.Zones.Sum(z => z.ObjectsCount);
        var totalObjectSpawnedCount = 0;
        foreach (var generationZone in _settings.Zones)
        {
            var spawnedObjectsCount = 0;
            var requiredObjects = generationZone.GenerationAssets.FindAll(a => a.IsExactSpawnCount);
            foreach (var generationAsset in requiredObjects)
            {
                for (var i = 0; i < generationAsset.SpawnCount; i++)
                {
                    yield return SpawnRadial(generationAsset, generationZone.MinR, generationZone.MaxR);
                    totalObjectSpawnedCount++;
                    spawnedObjectsCount++;
                    WorldGenerationProgress = totalObjectSpawnedCount / (float)totalObjectsToSpawn;
                }
            }

            var objectsToSpawn = generationZone.GenerationAssets.FindAll(a => !a.IsExactSpawnCount);
            while (spawnedObjectsCount < generationZone.ObjectsCount)
            {
                var generationAsset = objectsToSpawn[Random.Range(0, objectsToSpawn.Count)];
                yield return SpawnRadial(generationAsset, generationZone.MinR, generationZone.MaxR);
                totalObjectSpawnedCount++;
                spawnedObjectsCount++;
                WorldGenerationProgress = totalObjectSpawnedCount / (float)totalObjectsToSpawn;
            }
        }
    }

    private IEnumerator InitializationStep()
    {
        yield return null;
        
        foreach (var (worldObject, worldObjectData) in _generationResult)
        {
            worldObject.Initialize(worldObjectData);
        }

        Game.FileStorage.Set("worldData", _worldData);

        WorldGenerationProgress = 1f;

        yield return null;
    }

    private void OnObjectSpawned(GameObject levelObject, AssetReference assetRef)
    {
        var objectWorldData = new WorldObjectData
        {
            WorldPos = levelObject.transform.position,
            WorldRotation = levelObject.transform.rotation,
            PrefabAssetAddress = assetRef,
            ObjectSeed = Random.Range(1, int.MaxValue)
        };
                
        _worldData.WorldObjectsData.Add(objectWorldData);

        var worldObjectComponent = levelObject.GetComponent<WorldObject>();
        Debug.Assert(worldObjectComponent != null, $"levelObject {levelObject.name} missing WorldObject component!");
        
        _generationResult.Add(worldObjectComponent, objectWorldData);
    }

    private IEnumerator SpawnRadial(GenerationAsset genAsset, float minR, float maxR)
    {
        GameObject spawnResult = null;
        
        var assetRef = genAsset.AssetRef;
        var levelObjectHandle = assetRef.InstantiateAsync();
        while (!levelObjectHandle.IsDone)
        {
            yield return null;
        }
        
        var castResult = new List<Collider2D>();
        var levelObject = levelObjectHandle.Result;
        while (spawnResult == null)
        {
            castResult.Clear();
            var randomAngle = Random.Range(0f, 2 * math.PI);
            var randomR = Random.Range(minR, maxR);
            var pos = new Vector3(randomR * Mathf.Cos(randomAngle), randomR * Mathf.Sin(randomAngle), 0f);
            levelObject.transform.position = pos;
            levelObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(0f, 360f)));
            
            Physics2D.SyncTransforms();

            var obstaclesCastCollider = levelObject.GetComponent<Collider2D>();
            var filter = new ContactFilter2D { useLayerMask = true, layerMask = _settings.GenerationLayerMask };
            var castResultsCount = obstaclesCastCollider.OverlapCollider(filter, castResult);
            if (castResultsCount == 0)
            {
                spawnResult = levelObject;
            }
        }

        OnObjectSpawned(spawnResult, genAsset.AssetRef);
    }
}
