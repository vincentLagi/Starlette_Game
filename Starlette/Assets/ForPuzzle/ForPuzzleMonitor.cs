using UnityEngine;

public class ForPuzzleMonitor : MonoBehaviour, Interactable
{
    [SerializeField] ForPuzzleTask forPuzzleTask;
    [SerializeField] GameController gameController;
    public void Interact()
    {
        if (forPuzzleTask.GetIsDone())
        {
            forPuzzleTask.successErrorManagerScreen.SetStatusSuccesScreen(true);
            return;
        }
        forPuzzleTask.forPuzzleScreen.SetActive(true);
        gameController.SetState("OnPuzzle");
    }
}
