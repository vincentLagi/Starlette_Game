using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image itemImage;
    public Sprite selectedImage, deselectedImage;

    public void Awake()
    {
       Deselect();
    }

    public void Select(){
        itemImage.sprite = selectedImage;
    }

    public void Deselect(){
        itemImage.sprite = deselectedImage;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0){
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }

   
}
