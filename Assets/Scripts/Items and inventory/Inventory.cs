using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;



public class Inventory : MonoBehaviour,IsSaveManager
{
    public static Inventory Instance;

    
    
    public InventoryItem lastClickCraft;

    private UI_Manager uiManager;

    public float skillSoulAmount;//技能点数
    public Action<float> SkillSoulAmountChange;

    public List<ItemData> startingItem;

    public List<InventoryItem> equipment;   //穿戴装备
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;  //未穿戴装备
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List <InventoryItem> stash;   //物品
    public Dictionary <ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform craftSlotParent;

    private UI_itemSlot[] inventoryItemSlot;   //未穿戴装备放置槽
    private UI_itemSlot[] stashItemSlot;      //物品放置槽
    private UI_EquipmentSlot[] equipmentSlot;//穿戴装备放置槽
    private UI_itemSlot[] craftItemSlot; //合成物品槽

    private List<InventoryItem> loadInventory=new List<InventoryItem>();
    private List<InventoryItem> loadStash=new List<InventoryItem>();
    private List<InventoryItem > loadEquip=new List<InventoryItem>();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_itemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_itemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
    }

    private void Start()
    {
        uiManager = UI_Manager.Instance;

        uiManager.UpdateTextMeshList();

        craftItemSlot = craftSlotParent.GetComponentsInChildren<UI_itemSlot>();

        
        

    }


    public void LoadData(GameData _data)
    {
        if (_data != null)
        {
            this.skillSoulAmount = _data.skillSoulAmount;
            SkillSoulAmountChange?.Invoke(skillSoulAmount);
            ItemDataManager.Instance.GetItemDataList();

            foreach (var _pair in _data.inventory)
            {
                foreach (var _item in ItemDataManager.Instance.GetItemList())
                {
                    if (_item != null && _item.itemId == _pair.Key)
                    {
                        InventoryItem itemToLoad = new InventoryItem(_item);
                        itemToLoad.stackSize = _pair.Value;
                        loadInventory.Add(itemToLoad);
                    }
                }
            }

            foreach (var pair in _data.stash)
            {
                foreach (var _item in ItemDataManager.Instance.GetItemList())
                {
                    if (_item != null && _item.itemId == pair.Key)
                    {
                        InventoryItem itemToLoad = new InventoryItem(_item);
                        itemToLoad.stackSize = pair.Value;
                        loadStash.Add(itemToLoad);
                    }
                }
            }

            foreach (var pair in _data.equipment)
            {
                foreach (var _item in ItemDataManager.Instance.GetItemList())
                {
                    if (_item != null && _item.itemId == pair.Key)
                    {
                        InventoryItem itemToLoad = new InventoryItem(_item);
                        itemToLoad.stackSize = pair.Value;
                        loadEquip.Add(itemToLoad);
                    }
                }
            }
        }
        
        AddStartingItems();
    }

    public void SaveData(ref GameData _data)
    {
        _data.skillSoulAmount =skillSoulAmount;
        _data.inventory.Clear();
        _data.stash.Clear();
        _data.equipment.Clear();
        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId,pair.Value.stackSize);
        }

        foreach (var pair in stashDictionary)
        {
            _data.stash.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        
        foreach (var pair in equipmentDictionary)
        {
            _data.equipment.Add(pair.Key.itemId, pair.Value.stackSize);
        }

    }


    public void AddSkillSoul(int _amount)
    {
        skillSoulAmount += _amount;
        
        SkillSoulAmountChange?.Invoke(skillSoulAmount);
        
    }
    
    public bool SpendSkillSoul(float _amount)
    {
        if(skillSoulAmount<_amount)
            return false;
        else
        {
            skillSoulAmount -= _amount;
            SkillSoulAmountChange?.Invoke(skillSoulAmount);
            return true;
        }

    }

    private void AddStartingItems()
    {
        if (loadInventory.Count > 0 || loadStash.Count > 0||loadEquip.Count>0)
        {
            foreach (InventoryItem item in loadInventory)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }

            foreach (InventoryItem item in loadStash)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }

            foreach (InventoryItem item in loadEquip)
            {
                if (item.data is ItemData_Equipment _equip)
                {
                    EquipItem(item.data);
                }
            }

            return;
        }

        for (int i = 0; i < startingItem.Count; i++)
        {
            AddItem(startingItem[i]);
        }
    }


    //装备穿戴，传入穿戴装备数据进行插槽更换UI更新
    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;

            }
        }
        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        newEquipment.WearItemEffect();
        
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);
        
    }

    //将旧的穿戴装备卸下
    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if(itemToRemove == null)
            return;
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            
            equipment.Remove(value);
            itemToRemove.RemoveItemEffect();
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }


    //UI更新
    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);

                }
            }
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length;i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        if(uiManager != null) 
        uiManager.UpdateTextMeshList();

    }






    //对背包直接添加
    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipmet)
        {
            AddToInventory(_item);

        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }
        UpdateSlotUI();
        
    }

    //对物品槽的添加
    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }

        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    //对未穿戴装备槽添加
    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    //对物品槽与未穿戴装备槽进行移除
    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }


        UpdateSlotUI();
        
    }

   

    public List<InventoryItem> GetEquipmentList() => equipment; 

    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equipedItem = item.Key;
                break;
            }
        }
        return equipedItem;
    } 




    public bool CanCraft(ItemData_Equipment _itemCraft, List<InventoryItem> _equireMaterial)
    {
        
        
        List<InventoryItem> materialsToMove = new List<InventoryItem>();
        for (int i = 0; i < _equireMaterial.Count; i++)
        {
            if (stashDictionary.TryGetValue(_equireMaterial[i].data, out InventoryItem stashValue))
            {
               if (stashValue.stackSize < _equireMaterial[i].stackSize)
                {
                    
                    return false;
                    
                }
               else
                {
                    materialsToMove.Add(_equireMaterial[i]);
                }
            }
            else
            {
                
                return false;
            }
        }

        for (int i = 0; i < materialsToMove.Count; i++)
        {
            for (int j = 0; j < materialsToMove[i].stackSize; j++)
            {
                RemoveItem(materialsToMove[i].data);
            }
            
        }
        AddItem(_itemCraft);
        return true;
    }

    public bool CanCraftAgent(ItemData_Agent _itemCraft, List<InventoryItem> _agentMaterial)
    {

        List<InventoryItem> materialsToMove = new List<InventoryItem>();
        for (int i = 0; i < _agentMaterial.Count; i++)
        {
            if (stashDictionary.TryGetValue(_agentMaterial[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _agentMaterial[i].stackSize)
                {

                    return false;

                }
                else
                {
                    materialsToMove.Add(_agentMaterial[i]);
                }
            }
            else
            {

                return false;
            }
        }

        for (int i = 0; i < materialsToMove.Count; i++)
        {
            for (int j = 0; j < materialsToMove[i].stackSize; j++)
            {
                RemoveItem(materialsToMove[i].data);
            }

        }
        Debug.Log("222");
        AddItem(_itemCraft);
        return true;
    }


    public bool CanSureCraft(InventoryItem _craftObject,List<InventoryItem> _equireMaterial)
    {
        if (_craftObject == lastClickCraft)
            return true;
        else
        {
            int i = 0;
            foreach (var craftUI in craftItemSlot)
            {
                craftUI.CleanUpSlot();
                if (i < _equireMaterial.Count)
                {
                    craftUI.UpdateSlot(_equireMaterial[i]);
                }
                    
                i++;
            }
            

            lastClickCraft = _craftObject;
            return false;
        }
            
    }
}
