using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemDataManager : MonoBehaviour
{
    public static ItemDataManager Instance;
    private List<ItemData> itemDataList = new List<ItemData>();
    public string[] assetName;
    public List<ItemData> loadItems;
    [SerializeField] private List<ItemData> itemDataEquipmentWeapoonList = new List<ItemData>();
    [SerializeField] private List<ItemData> itemDataEquipmentArmorList = new List<ItemData>();
    [SerializeField] private List<ItemData> itemDataEquipmentAmuletList = new List<ItemData>();
    [SerializeField] private List<ItemData> itemDataEquipmentFlaskList = new List<ItemData>();
    [SerializeField] private List<ItemData> itemDataEquipmentAgentList = new List<ItemData>();
    [SerializeField]private List<ItemData> itemDataMaterialList=new List<ItemData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    public List<ItemData> GetList(craftTypeSelect  _typeSelect)
    {
        
        if (_typeSelect == craftTypeSelect.weapon)
        {
            return itemDataEquipmentWeapoonList ;
        }
        else if (_typeSelect == craftTypeSelect.armor)
        {
            return itemDataEquipmentArmorList;
        }
        else if (_typeSelect == craftTypeSelect.amulet)
        {
            return itemDataEquipmentAmuletList;
        }
        else if (_typeSelect == craftTypeSelect.flask)
        {
            
            return itemDataEquipmentFlaskList;
        }
        else if (_typeSelect == craftTypeSelect.agent)
        {
            return itemDataEquipmentAgentList;
        }
        return null;
    }

#if UNITY_EDITOR
    public List<ItemData> GetItemDataList()
    {
        itemDataList = new List<ItemData>();
        assetName = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/Data/Equipments" });
        foreach (string SOName in assetName)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataList.Add(itemData);
        }

        assetName = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/Data/Materials" });
        foreach (string SOName in assetName)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataList.Add(itemData);
        }

        SelectItemData();
        return GetItemList();
    }
#endif
    public List<ItemData> GetItemList()
    {
        return itemDataList;
    }

    private void SelectItemData()
    {
        itemDataEquipmentWeapoonList.Clear();
        itemDataEquipmentFlaskList.Clear();
        itemDataEquipmentArmorList.Clear();
        itemDataEquipmentAmuletList.Clear();
        itemDataEquipmentAgentList.Clear();
        itemDataMaterialList.Clear();

        foreach (ItemData _itemData in itemDataList)
        {

            if (_itemData is ItemData_Equipment _equip)
            {
                switch (_equip.equipmentType)
                {
                    case EquipmentType.Weapon:
                        itemDataEquipmentWeapoonList.Add(_itemData);
                        break;

                     case EquipmentType.Armor:
                        itemDataEquipmentArmorList.Add(_itemData);
                        break;

                    case EquipmentType.Amulet:
                        itemDataEquipmentAmuletList.Add(_itemData);
                        break;
                    case EquipmentType.Flask:
                        itemDataEquipmentFlaskList.Add(_itemData);
                        break;





                }




            }

            else
            {
                if (_itemData is ItemData_Agent _agent)
                {
                    itemDataEquipmentAgentList.Add(_itemData);
                }

                else
                    itemDataMaterialList.Add(_itemData);
            }


        }
    }


}
