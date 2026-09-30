using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLooseItems;
    [SerializeField] private float chanceToLooseMaterial;

    public override void GenerateDrop()
    {
        Inventory _inventory=Inventory.Instance;

        
        List<InventoryItem> itemToUnequip=new List<InventoryItem> ();
        List<InventoryItem> materialsToLoose = new List<InventoryItem>();

        foreach (InventoryItem item in _inventory.GetStashList())
        {
            
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                
                Dropitem(item.data);
                
                itemToUnequip.Add(item);
                
            }
        }
        

        for (int i = 0; i < itemToUnequip.Count; i++)
        {
            _inventory.UnequipItem(itemToUnequip[i].data as ItemData_Equipment);
            
        }

        foreach (InventoryItem item in _inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToLooseMaterial)
            {

                Dropitem(item.data);

                materialsToLoose.Add(item);

            }

            for (int i = 0; i < materialsToLoose.Count; i++)
            {
                _inventory.RemoveItem(materialsToLoose[i].data);
            }
        }
    }
}
