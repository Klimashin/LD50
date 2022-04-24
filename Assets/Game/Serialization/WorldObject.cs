using UnityEngine;

public class WorldObject : MonoBehaviour
{
    private WorldObjectData _data;

    private int _seed;
    
    public void Initialize(WorldObjectData data)
    {
        _data = data;
        var t = transform;
        t.position = data.WorldPos;
        t.rotation = data.WorldRotation;

        _seed = data.ObjectSeed;
        
        Random.InitState(_seed);

        var children = GetComponentsInChildren<IWorldObjectRandomDependent>();
        foreach (var worldObjectRandomDependent in children)
        {
            worldObjectRandomDependent.Initialize( RollD100() );
        }
    }

    private int RollD100()
    {
        return Random.Range(1, 101);
    }
}

public interface IWorldObjectRandomDependent
{
    public void Initialize(int roll);
}
