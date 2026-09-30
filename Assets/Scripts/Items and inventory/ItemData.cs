using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public enum ItemType
{
    Material,
    Equipmet
}

[CreateAssetMenu(fileName ="New Item Data",menuName ="Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public string itemId;

    [Range(0,100)]
    public float dropChance;

    public virtual void UseConsumable()
    {

    }

    public void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);

#endif
    }

}
