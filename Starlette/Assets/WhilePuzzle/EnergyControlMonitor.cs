using UnityEngine;

public class EnergyControlMonitor : MonoBehaviour, Interactable
{
    public GameObject controlEnergyScreen;
    public GameController gameController;
    public void Interact()
    {
        controlEnergyScreen.SetActive(true);
        gameController.SetState("OnPuzzle");
    }

}
