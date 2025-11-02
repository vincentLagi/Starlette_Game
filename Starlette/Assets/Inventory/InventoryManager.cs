using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    int selectedSlot = -1;

    public void Start()
    {
        ChangeSelectedSlot(0);
    }

    public void Update()
    {
       if(Input.inputString != null){
        bool isNumber = int.TryParse(Input.inputString, out int number);
        if(isNumber && number > 0 && number < 8){
            ChangeSelectedSlot(number-1);
        }
       }
    }

    void ChangeSelectedSlot(int newValue){
        if(selectedSlot >=0) inventorySlots[selectedSlot].Deselect();
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public void AddItem(Item item){
        InventorySlot check= CheckInventory();
       if(check == null){
            //Debug.Log("Inventory full");
       }else{
            SpawnNewItem(item, check);
       }
        
    }

    public Item getSelectedItem(){
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if(itemInSlot != null){
            return itemInSlot.item;
        }
        return null;
    }

    InventorySlot CheckInventory(){
        for (int i = 0; i < inventorySlots.Length; i++){
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot == null){
                return slot;
            }
        }
        return null;
    }

    void SpawnNewItem(Item item, InventorySlot slot){
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    
}
