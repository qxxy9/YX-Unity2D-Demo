using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public float skillSoulAmount;

    public SerializableDictionary<string, int> inventory;
    public SerializableDictionary<string, int> stash;
    public SerializableDictionary<string, int> equipment;
    public SerializableDictionary<string, TargetSaveData> targetData;
    public List<string> skillUnlocked;
    public List<string> skillUsed;

    public GameData()
    {
        this.skillSoulAmount = 0;
        inventory= new SerializableDictionary<string, int>();
        stash= new SerializableDictionary<string, int>();
        equipment= new SerializableDictionary<string, int>();
        targetData = new SerializableDictionary<string, TargetSaveData>();
    }

}
