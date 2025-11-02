using UnityEngine;

public class ComputerTable : MonoBehaviour, Interactable
{
    
    public Item item;
    
    public InventoryManager inventoryManager;
    public void Interact()
    {
       Debug.Log("Ini ComputerTable");
        inventoryManager.AddItem(item);
    }

 
}
