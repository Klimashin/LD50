using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "GeneratorSettings", menuName = "ScriptableObjects/GeneratorSettingsSO", order = 1)]
public class LevelGeneratorSettings : ScriptableObject
{
    public float GenerationSize = 40f;
    public int GenerationCount = 200;
    public List<AssetReference> GenerationPrefabs;
    public LayerMask GenerationLayerMask;
}
