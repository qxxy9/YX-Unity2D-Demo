using UnityEngine;
using UnityEngine.EventSystems;



public class UI_ItemSlot_Select : UI_itemSlot
{
     [SerializeField]private CanvasGroup _object;
     

    private void Start()
    {
        _object.alpha = 0;
        _object.interactable = false;
        _object.blocksRaycasts = false;

        
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        
        UI_Manager.Instance.TurnSeleceUI(_object);
    }
}
