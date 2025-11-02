using UnityEngine;

public class Fridge : MonoBehaviour, Interactable
{
    public Item item;
    
    public InventoryManager inventoryManager;

    public void Interact()
    {
        Debug.Log("Ini Fridge");
        inventoryManager.AddItem(item);
    }


}
