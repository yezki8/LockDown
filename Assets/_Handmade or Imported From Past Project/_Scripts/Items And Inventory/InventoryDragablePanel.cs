using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDragablePanel : MonoBehaviour
{
    [SerializeField] CanvasGroup ThisCanvasGroup;
    public ItemSO DraggedItem;
    public InventorySlotController ItemSource;
    public InventorySlotController ItemDestination;

    public static InventoryDragablePanel Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    public void DraggingAnItem(ItemSO dragSO, InventorySlotController source)
    {
        ItemSource = source;
        ThisCanvasGroup.alpha = 1;
        DraggedItem = dragSO;
        this.GetComponent<Image>().sprite = dragSO.ItemSprite;
    }

    public void EndDragItem()
    {
        if (ItemDestination != null)
        {
            if (ItemDestination.TypeOfSlot == InventorySlotController.SlotType.Free)
            {
                if (ItemDestination.ItemStored == null)
                {
                    ItemSource.RemoveItem();
                    ItemDestination.StoreItem(DraggedItem);
                }
                else
                {
                    ItemSource.StoreItem(ItemDestination.ItemStored);
                    ItemDestination.StoreItem(DraggedItem);
                }
            }
            else if (ItemDestination.TypeOfSlot == InventorySlotController.SlotType.Melle)
            {
                if (ItemDestination.ItemStored == null && DraggedItem.TypeOfHit == HitType.melle)
                {
                    ItemSource.RemoveItem();
                    ItemDestination.StoreItem(DraggedItem);
                }
                else if (ItemDestination.ItemStored != null && DraggedItem.TypeOfHit == HitType.melle)
                {
                    ItemSource.StoreItem(ItemDestination.ItemStored);
                    ItemDestination.StoreItem(DraggedItem);
                }
            }
            else
            {
                if (ItemDestination.ItemStored == null && DraggedItem.TypeOfHit == HitType.range)
                {
                    ItemSource.RemoveItem();
                    ItemDestination.StoreItem(DraggedItem);
                }
                else if (ItemDestination.ItemStored != null && DraggedItem.TypeOfHit == HitType.range)
                {
                    ItemSource.StoreItem(ItemDestination.ItemStored);
                    ItemDestination.StoreItem(DraggedItem);
                }
            }            
        }
        ItemSource = null;
        ItemDestination = null;
        DraggedItem = null;
        ThisCanvasGroup.alpha = 0;
    }

    public void FollowMouse()
    {
        Vector3 posToFollow = Input.mousePosition;
        Vector3 targetPos = posToFollow;                //y value of offset in front of the target

        //Lerping the follow speed so the movement won't be stiff
        this.transform.position = Vector3.Lerp(this.transform.position,
            targetPos,
            10 * Time.deltaTime);
    }
}
