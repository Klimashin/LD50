using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CampSystem", menuName = "Systems/CampSystem")]
public class CampSystem : ScriptableObject
{
    public CharacterNameToCharacterDataDict Characters;
    
    [ShowInInspector] public int CurrentFood { get; set; }
    [ShowInInspector] public int CurrentDay { get; set; }

    private void Awake()
    {
        CurrentFood = 0;
        CurrentDay = 0;
    }
}

[Serializable]
public class CharacterNameToCharacterDataDict : UnitySerializedDictionary<string, CharacterData> {}

[Serializable]
public class CharacterData
{
    public string Name;
    public bool IsAlive;
    public bool IsFed;
    public int FoodRequired;
}
