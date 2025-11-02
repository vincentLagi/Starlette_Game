using UnityEngine;

public class WhilePuzzleMonitor : MonoBehaviour, Interactable
{
    [SerializeField] WhilePuzzleTask whilePuzzleTask;
    [SerializeField] GameController gameController;
    public void Interact()
    {
        if (whilePuzzleTask.GetIsDone())
        {
            whilePuzzleTask.successErrorManagerScreen.SetStatusSuccesScreen(true);
            return;
        }
        gameController.SetState("OnPuzzle");
        whilePuzzleTask.whilePuzzleScreen.SetActive(true);
    }
}
