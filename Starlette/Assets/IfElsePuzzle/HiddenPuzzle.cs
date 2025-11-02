using UnityEngine;

public class HiddenPuzzle : MonoBehaviour, Interactable
{
    public Check2Monitor check2Monitor;
    public void Interact()
    {
        Debug.Log("hidden puzzle");
        check2Monitor.ResetPuzzle();
        check2Monitor.SetIsShowHiddenPuzzle(true);
    }

   
}
