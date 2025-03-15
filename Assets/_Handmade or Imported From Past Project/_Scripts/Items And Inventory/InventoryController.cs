using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [System.Serializable]
    public class InventorySlot
    {
        public ItemSO ItemStored;
        public Image SlotImage;
    }
    public InventorySlot[] BagContent;          //Inside traditional Inventory
    public InventorySlot[] PocketContent;       //Slots for quick equip

    public static InventoryController Instance;

    private void Start()
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

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void AddItem(GameObject targetItem)
    {
        ItemSO collectedItem = targetItem.GetComponent<ItemController>().
            ItemScriptableObject;
        
        for (int i = 0; i < BagContent.Length; i++)
        {
            if (BagContent[i].ItemStored == null)
            {
                BagContent[i].ItemStored = collectedItem;
                Debug.Log($"{targetItem.name} was added to slot {i} of the bag");
                Destroy(targetItem);
                break;
            }
        }
        ////Checking inside bag
        //foreach (var slot in BagContent)
        //{
        //    if (slot.ItemStored == null)
        //    {
        //        slot.ItemStored = collectedItem;
        //        GameObject.Destroy(targetItem);
        //        break;
        //    }
        //}
    }
}
