using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "GeneratorSettings", menuName = "ScriptableObjects/GeneratorSettingsSO", order = 1)]
public class LevelGeneratorSettings : ScriptableObject
{
    public List<GenerationZone> Zones;
    public LayerMask GenerationLayerMask;
}

[Serializable]
public class GenerationZone
{
    public List<GenerationAsset> GenerationAssets;
    public ZoneType Type;
    public float MinR;
    public float MaxR;
    public int ObjectsCount;
}

public enum ZoneType
{
    Radial,
    River
}

[Serializable]
public class GenerationAsset
{
    public AssetReference AssetRef;
    public bool IsMaxCount;
    [ShowIf("IsMaxCount")] public int MaxCount;
    public bool IsExactSpawnCount;
    [ShowIf("IsExactSpawnCount")] public int SpawnCount;
}
