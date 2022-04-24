using System.Linq;
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

        var objectSeedDependencies = GetComponentsInChildren<IRandomSeedDependent>();
        foreach (var seedDependent in objectSeedDependencies)
        {
            seedDependent.Initialize( RollD100() );
        }

        var idObjects = GetComponentsInChildren<ObjectID>().ToList();
        foreach (var historyEvent in _data.HistoryEventsLog)
        {
            var target = idObjects.Find(obj => obj.ID == historyEvent.SourceID);
            historyEvent.Apply(target.gameObject);
        }
    }

    public void AddHistoryEvent(HistoryEvent e)
    {
        _data.HistoryEventsLog.Add(e);
    }

    private int RollD100()
    {
        return Random.Range(1, 101);
    }
}

public interface IRandomSeedDependent
{
    public void Initialize(int roll);
}
