using UnityEngine;
using UnityEngine.UI;

public class OpenCloseBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Button closeButton;
    [SerializeField] private GameController gameController;
    void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
        if (gameController == null)
        {
            Debug.LogError("GameController not found in the scene.");
            return;
        }
        Debug.Log(gameController.name);
        
    }

    public void ClosePanel()
    {
        // get all direct children of this panel then set them off

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
        gameController.SetState("FreeRoam");
    }

    public void OpenPanel(string panelName = "")
    {
        if (gameController == null)
        {

            Debug.LogError("GameController is not assigned." + gameController.gameObject.name);
            return;
        }
        gameController.SetState("OnPuzzle");
        Debug.Log($"Opening panel: {panelName}");
        // find the name of Gameobject of the direct child, then set it active
        Transform button = transform.Find("CloseButton");
        transform.gameObject.SetActive(true); // Ensure the panel is active before showing it
        if (button != null)
        {
            button.gameObject.SetActive(true); 
        }
        else
        {
            Debug.LogWarning("CloseButton not found in OpenCloseBehavior.");
        }

        if (string.IsNullOrEmpty(panelName))
        {
            return; // No panel name provided, do nothing
        }
        transform.Find(panelName).gameObject.SetActive(true);
    }
}
