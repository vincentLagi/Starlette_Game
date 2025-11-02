using UnityEngine;

public class ActivateEnding : MonoBehaviour, Interactable
{
    public GameObject endingScene;

    public void Interact()
    {
        endingScene.SetActive(true);
    }
}
