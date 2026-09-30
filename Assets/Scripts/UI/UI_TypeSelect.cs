using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum craftTypeSelect
{
    weapon,
    armor,
    amulet,
    flask,
    agent
}

public class UI_TypeSelect : UI_itemSlot
{
    [SerializeField]private CanvasGroup checkUI;
    [SerializeField]private craftTypeSelect craftTypeSelect;
    private List<InventoryItem> _inventoryItem=new List<InventoryItem>();
    private List<ItemData> _itemData=new List<ItemData>();
    private UI_itemSlot[] itemSlotList;

    private void Start()
    {
        checkUI.alpha = 0;
        checkUI.interactable = false;
        checkUI.blocksRaycasts = false;
        itemSlotList =checkUI.GetComponentsInChildren<UI_itemSlot>();
        _itemData = ItemDataManager.Instance.GetList(craftTypeSelect);
        
        foreach (var item in _itemData)
        {

            InventoryItem inventoryItem = new InventoryItem(item);
            _inventoryItem.Add(inventoryItem);
        }

        CheckItemUpdate();

    }



    public override void OnPointerDown(PointerEventData eventData)
    {
        UI_Manager.Instance.TurnCraftSeleceUI(checkUI);

    }

    private void CheckItemUpdate()
    {
        int i = 0;
        foreach (var itemSlot in itemSlotList)
        {
            itemSlot.CleanUpSlot();
            if (i < _inventoryItem.Count)
            {
               itemSlot.UpdateSlot(_inventoryItem[i]);
            }
            
            i++;

        }
    }
}
