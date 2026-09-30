using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_itemSlot : MonoBehaviour,IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]private Image itemImage;
    [SerializeField]private TextMeshProUGUI itemText;
    public TextMeshProUGUI troduceText;

    public InventoryItem item;
    // Start is called before the first frame update
   

    public void UpdateSlot(InventoryItem _item)
    {
        item= _item;
        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite= null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        if (item .data!= null)
       {
            if (Input.GetKey(KeyCode.LeftControl) && TimePuaseManager.instance.CanOpenBackpack())
            {
                Inventory.Instance.RemoveItem(item.data);
                return;
                    
            }

            if (item.data is ItemData_Agent)
            {
                item.data.UseConsumable();
                Inventory.Instance.RemoveItem(item.data);
                return;
            }

        if (item.data.itemType == ItemType.Equipmet)
            Inventory.Instance.EquipItem(item.data);

            troduceText.text = "";
        }
        else return;
    }


    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;

        if (item.data == null)
        {
            
            return;
        }
            

        if (troduceText != null)
        {
            troduceText.text = item.data.itemName;
        }
        

    }

    // 鼠标离开
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if(item == null)
            return ;

        if (item.data == null)
        {
            
            return;
        }
            

        if (troduceText != null )
        {
            troduceText.text = "";
        }
        // 隐藏提示面板、恢复大小、恢复颜色
    }

}
