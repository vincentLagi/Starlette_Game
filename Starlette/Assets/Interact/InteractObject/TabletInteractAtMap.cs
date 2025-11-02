using System;
using UnityEngine;

public class TabletInteractAtMap : MonoBehaviour, Interactable
{
    public Item item;
    public InventoryManager inventoryManager;
    public TabletManager tabletManager;
    public GameController gameController;
    private String username;
    public void Interact()
    {
        //Debug.Log("Ini Fridge");
        inventoryManager.AddItem(item);
        gameController.SetState("OnTablet");
        tabletManager.setStatusDialog1Interface(true);
        tabletManager.setStatusTabletInterface(true);
        Destroy(gameObject);
    }

   
}
