using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CampSystem", menuName = "Systems/CampSystem")]
public class CampSystem : ScriptableObject
{
    public int InitialFood;
    public CharacterNameToCharacterDataDict Characters;
    
    [ShowInInspector] public int CurrentFood { get; set; }
    [ShowInInspector] public int CurrentDay { get; set; }

    public void Reset()
    {
        CurrentFood = InitialFood;
        CurrentDay = 0;
        
        foreach (var characterData in Characters.Values)
        {
            characterData.IsAlive = true;
            characterData.IsFed = false;
        }
    }

    public void InitFromWorldData(WorldData data)
    {
        CurrentFood = data.CurrentFood;
        CurrentDay = data.CurrentDay;
        
        foreach (var characterName in Characters.Keys)
        {
            if (data.DeadCharacters.Contains(characterName))
            {
                Characters[characterName].IsAlive = false;
            }
        }
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
    public string[] SpeechArray;
}
