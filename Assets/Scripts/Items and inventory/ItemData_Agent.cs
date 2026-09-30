using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum objectType
{
    material,
    consumable

}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Agent")]
public class ItemData_Agent : ItemData
{
    public objectType objectType;
    [SerializeField]private ItemEffect _itemEffect;

    [Header("Craft requirement")]
    public List<InventoryItem> craftMaterial;

    public override void UseConsumable()
    {
        
        _itemEffect.ExcuteEffect( null);

    }
}
