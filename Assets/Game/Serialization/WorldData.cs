using System;
using System.Collections.Generic;

[Serializable]
public class WorldData
{
    public int WorldSeed;

    public int CurrentDay;

    public int CurrentFood;

    public HashSet<string> DeadCharacters = new HashSet<string>();

    public List<WorldObjectData> WorldObjectsData;
}
