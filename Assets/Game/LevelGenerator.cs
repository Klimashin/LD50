using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public float GenerationSize = 40f;
    public int GenerationCount = 200;
    public List<GameObject> GenerationPrefabs;

    public IEnumerator Generate()
    {
        yield return null;
        
        var haltonSeq = new HaltonSequence();
        haltonSeq.Reset();

        var pos = Vector3.zero;
        var posOffset = new Vector3(GenerationSize / 2f, GenerationSize / 2f, 0f);

        for(var i=0; i < GenerationCount; i++)
        {
            haltonSeq.Increment();
            
            pos = haltonSeq.m_CurrentPos;
            pos.z = 0.0f;
            pos *= GenerationSize;

            var prefab = GenerationPrefabs[Random.Range(0, GenerationPrefabs.Count)];
            var levelObject = Instantiate(prefab);
            levelObject.transform.position = pos - posOffset;
            levelObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(0f, 360f)));
        }
    }
}
