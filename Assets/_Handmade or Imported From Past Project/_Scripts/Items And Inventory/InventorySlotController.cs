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

    [Header("For Tooltip")]
    public float CallTooltipTimer = 4;
    [SerializeField] float _callTooltipCountdown;
    [SerializeField] bool isCallingTooltip;

    public enum SlotType
    {
        Free,
        Melle,
        Range
    }
    public SlotType TypeOfSlot;

    private void Update()
    {
        if (isCallingTooltip)
        {
            if (_callTooltipCountdown > 0)
            {
                _callTooltipCountdown -= 1 * Time.deltaTime;
            }
            else
            {
                CallTooltip();
            }
        }
        else
        {
            _callTooltipCountdown = CallTooltipTimer;
        }
    }

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

    void CallTooltip()
    {
        isCallingTooltip = false;
        TooltipController.Instance.SetParagraph(ItemStored.ItemName, ItemStored.ItemDescription);
        TooltipController.Instance.ShowTooltip();
    }

    // Event System =============================================================================
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryDragablePanel.Instance.ItemDestination = this;
        if (ItemStored != null)
        {
            isCallingTooltip = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (InventoryDragablePanel.Instance.ItemDestination == this)
        {
            InventoryDragablePanel.Instance.ItemDestination = null;
        }
        isCallingTooltip = false;
        TooltipController.Instance.HideTooltip();
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
