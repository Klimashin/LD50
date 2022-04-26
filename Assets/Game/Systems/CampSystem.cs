using System;
using System.Collections.Generic;
using System.Linq;
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

    public void StartCampPhase()
    {
        foreach (var charData in Characters.Values)
        {
            charData.IsFed = false;   
        }
        
        var worldData = Game.FileStorage.Get<WorldData>("worldData");
        worldData.CurrentDay = CurrentDay;
        worldData.CurrentFood = CurrentFood;
        worldData.DeadCharacters =
            new HashSet<string>(Characters.Values.Where(character => !character.IsAlive).Select(c => c.Name));
        
        Game.FileStorage.Save();
        
        CurrentDay++;
    }

    public List<string> EndCampPhase()
    {
        return KillStarvingCharacters();
    }

    public void InitFromWorldData(WorldData data)
    {
        Reset();
        
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
    
    public bool HasAliveCharacters()
    {
        return Characters.Values.ToList().Exists(character => character.IsAlive);
    }
    
    private List<string> KillStarvingCharacters()
    {
        var result = new List<string>();
        foreach (var character in Characters.Values.Where(character => character.IsAlive && !character.IsFed))
        {
            character.IsAlive = false;
            result.Add(character.Name);
        }

        return result;
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
