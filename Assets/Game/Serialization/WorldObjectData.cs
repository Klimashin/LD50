using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class WorldObjectData
{
    public AssetReference PrefabAssetAddress;
    public Vector3 WorldPos;
    public Quaternion WorldRotation;
}
