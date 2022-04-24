using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectID : MonoBehaviour
{
    public int ID;

    [Button]
    private void NewID()
    {
        ID = Mathf.Abs(Guid.NewGuid().GetHashCode());
    }
    
    private void OnValidate()
    {
        if (ID == 0)
        {
            ID = Mathf.Abs(Guid.NewGuid().GetHashCode());
        }
    }
}
