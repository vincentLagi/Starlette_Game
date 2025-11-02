using UnityEngine;

public class MonitorRoom : MonoBehaviour, Interactable
{
    public GameController gameController;
    public GameObject puzzleInterface;
    public void Interact()
    {
        gameController.SetState("OnPuzzle");
        if (puzzleInterface.transform.parent.GetComponent<IfElseTask>().getIsComplete())
        {
            puzzleInterface.transform.parent.GetComponent<IfElseTask>().successErrorManagerScreen.SetStatusSuccesScreen(true);
            return;
        }
        puzzleInterface.SetActive(true);
    }

   
}
