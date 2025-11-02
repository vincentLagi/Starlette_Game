using Unity.VisualScripting;
using UnityEngine;


public class PanelInteractBehavior : MonoBehaviour, Interactable
{
    [SerializeField] public GameObject panelInterface;
    [SerializeField] public GameObject panelChild;
    [SerializeField] public GameObject backgroundPanel;
    public void Interact()
    {
        if (panelInterface.name == "SecondRoomFirstPartPuzzle" && panelInterface.GetComponent<FirstPart>().GetIsDone())
        {
            panelInterface.GetComponent<FirstPart>().successErrorManagerScreen.SetStatusSuccesScreen(true);
            return;
        }
        else if (panelInterface.name == "SecondRoomSecondPartPuzzle" && panelInterface.GetComponent<SecondPart>().GetIsDone())
        {
            panelInterface.GetComponent<SecondPart>().successErrorManagerScreen.SetStatusSuccesScreen(true);
            return;
        }
        if (panelInterface == null)
        {
            Debug.LogError("Panel Interface is not assigned.");
            return;
        }
        // Debug.Log($"Interacting with panel: {panelInterface.name}");
        if (panelChild == null)
        {
            backgroundPanel.GetComponent<OpenCloseBehavior>().OpenPanel(panelInterface.name);
            GameObject parttwo = GameObject.Find("SecondRoomSecondPartPuzzle").gameObject;
            parttwo.SetActive(false);
        }
        else
        {
            backgroundPanel.GetComponent<OpenCloseBehavior>().OpenPanel(panelInterface.name);
            foreach (Transform child in panelInterface.transform)
            {
                if (child.gameObject.name == panelChild.name)
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        
    }
}