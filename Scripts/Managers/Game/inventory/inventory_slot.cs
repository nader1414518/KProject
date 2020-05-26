using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory_slot : MonoBehaviour
{
    public bool bIsSlotReserved = false;
    public Item slotItem;

    void OnEnable()
    {
        if (this.GetComponent<Image>().sprite != this.GetComponentInParent<inventory_controller>().nullPhoto)
        {
            bIsSlotReserved = true;
        }
        else
        {
            bIsSlotReserved = false;
        }
    }

    public void DiscardItem()
    {
        // Remove the item from the database 
        if (game_globals.playerUsername != "" && game_globals.playerUsername != null)
        {
            if (game_globals.playerItems != null)
            {
                if (slotItem)
                {
                    game_db_manager.DeleteItem(game_globals.playerUsername, slotItem.name, slotItem.id);
                }
            }
        }
        slotItem = null;
        this.GetComponent<Image>().sprite = this.GetComponentInParent<inventory_controller>().nullPhoto;
        bIsSlotReserved = false;
    }
}
