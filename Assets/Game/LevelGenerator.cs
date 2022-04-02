using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GenerationCircle> _circles;
    
    public IEnumerator Generate()
    {
        yield return null;
        
        foreach (var generationCircle in _circles)
        {
            yield return null;
        }
    }

    private void CircleGeneration(GenerationCircle data)
    {
        UnityEngine.Rendering.HaltonSequence.Get(0, 0);
    }
}

[Serializable]
public class GenerationCircle
{
    public float MinR;
    public float MaxR;
    public List<GameObject> Prefabs;
    public float Step;
}
