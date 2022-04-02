using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CampSystem", menuName = "Systems/CampSystem")]
public class CampSystem : ScriptableObject
{
    public CharacterNameToCharacterDataDict Characters;
    
    public int CurrentFood { get; set; }
    public int CurrentDay { get; set; }
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
