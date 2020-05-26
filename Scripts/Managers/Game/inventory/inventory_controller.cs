using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory_controller : MonoBehaviour
{
    [Tooltip("The photo is demonstrates that the slot is empty")]
    public Sprite nullPhoto;
    [Tooltip("The children inventory slots")]
    public inventory_slot[] inventorySlots;
    [Tooltip("The string types of the items (names of the items)")]
    public List<Item> itemTypes;

    //public Image testImg;

    void OnEnable()
    {
        inventorySlots = this.GetComponentsInChildren<inventory_slot>();
        RetrieveInventoryFromDB(game_globals.playerUsername);
    }

    public bool AddItemToInventory(Item item)
    {
        if (item)
        {
            Debug.Log("Item is not null ... ");
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].GetComponent<Image>().sprite == nullPhoto)
                {
                    inventorySlots[i].GetComponent<Image>().sprite = item.icon;
                    inventorySlots[i].slotItem = item;
                    inventorySlots[i].bIsSlotReserved = true;
                    game_db_manager.InsertItem(game_globals.playerUsername, item.name, item.category);
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    public void RetrieveInventoryFromDB(string username)
    {
        if (username != "" && username != null)
        {
            List<Item> items = game_db_manager.GetItemsForPlayer(username);
            for (int m= 0; m < items.Count; m++)
            {
                for (int n = 0; n < itemTypes.Count; n++)
                {
                    if (items[m].name == itemTypes[n].name)
                    {
                        items[m].icon = itemTypes[n].icon;
                        //Debug.Log("Retrieved Item from database ... ");
                    }
                }
            }
            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < inventorySlots.Length; j++)
                {
                    if (inventorySlots[i].GetComponent<Image>().sprite == nullPhoto)
                    {
                        inventorySlots[i].slotItem = items[i];
                        inventorySlots[i].GetComponent<Image>().sprite = items[i].icon;
                        inventorySlots[i].bIsSlotReserved = true;
                        //Debug.Log("Assigned Inventory Slot ... ");
                        break;
                    }
                }
            }
            Debug.Log("Retrieved Items from the Database ... ");
        }
    }
}
