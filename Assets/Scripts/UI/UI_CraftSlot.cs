using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_itemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        
        
        ItemData_Equipment craftData=item.data as ItemData_Equipment;
        ItemData_Agent craftDataAgent = item.data as ItemData_Agent;
        
        if (item.data is ItemData_Agent)
        {
            if(!Inventory.Instance.CanSureCraft(item,craftDataAgent.craftMaterial))
                return ;
            Inventory.Instance.CanCraftAgent(craftDataAgent, craftDataAgent.craftMaterial);
           
        }
        else
        {

            if (!Inventory.Instance.CanSureCraft(item, craftData.craftMaterial))
                return;
            Inventory.Instance.CanCraft(craftData, craftData.craftMaterial);
        }
        
        
    }
}
