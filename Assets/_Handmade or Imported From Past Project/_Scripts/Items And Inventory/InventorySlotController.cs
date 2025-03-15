using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotController : MonoBehaviour, IPointerDownHandler, 
    IDragHandler, IEndDragHandler, IBeginDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    public ItemSO ItemStored;
    public Image SlotImage;

    public enum SlotType
    {
        Free,
        Melle,
        Range
    }
    public SlotType TypeOfSlot;

    public void StoreItem(ItemSO item)
    {
        ItemStored = item;
        SlotImage.sprite = ItemStored.ItemSprite;
    }

    public void RemoveItem()
    {
        ItemStored = null;
        SlotImage.sprite = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryDragablePanel.Instance.ItemDestination = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (InventoryDragablePanel.Instance.ItemDestination == this)
        {
            InventoryDragablePanel.Instance.ItemDestination = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Dragging");
        InventoryDragablePanel.Instance.EndDragItem();

        var tempColor = SlotImage.color;
        tempColor.a = 1f;
        SlotImage.color = tempColor;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Dragging");
        if (ItemStored != null)
        {
            InventoryDragablePanel.Instance.DraggingAnItem(ItemStored, this);

            var tempColor = SlotImage.color;
            tempColor.a = 0.5f;
            SlotImage.color = tempColor;
        }
    }
}
