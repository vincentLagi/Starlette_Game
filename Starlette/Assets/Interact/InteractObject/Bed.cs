using UnityEngine;

public class Bed : MonoBehaviour, Interactable
{
    
    // public bool CanInteract()
    // {
    //     return !canInteract;
    // }
    [SerializeField] Dialog dialog;
    public void Interact()
    {
        // if(!CanInteract())return;
        // do something
        Debug.Log("Ini bed");
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }

        


}
