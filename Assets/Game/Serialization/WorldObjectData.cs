using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class WorldObjectData
{
    public AssetReference PrefabAssetAddress;
    public Vector3 WorldPos;
    public Quaternion WorldRotation;
    public int ObjectSeed;
    public readonly List<HistoryEvent> HistoryEventsLog = new();
}

[Serializable]
public abstract class HistoryEvent
{
    protected HistoryEvent(int sourceID)
    {
        SourceID = sourceID;
    }
    
    public int SourceID;
    
    public abstract void Apply(GameObject target);
}

[Serializable]
public class HistoryDisableEvent : HistoryEvent
{
    public override void Apply(GameObject target)
    {
        target.SetActive(false);
    }

    public HistoryDisableEvent(int sourceID) : base(sourceID) { }
}
