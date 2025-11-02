using UnityEngine;

public class DoorMonitorRoomOne : MonoBehaviour, Interactable
{
    public WireTask wireTask;
    public GameController gameController;
    public GameObject toolBag;
    public void Interact()
    {
        gameController.SetState("OnPuzzle");
        toolBag.SetActive(false);
        wireTask.setStatusWirePuzzleInterface(true);
    }

  
}
