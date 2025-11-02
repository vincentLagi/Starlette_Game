using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPuzzle : MonoBehaviour
{
    public GameObject[] listChoiceButton;
    [SerializeField] private DialogueSystem dialogueSystem;
    public Sprite defaultChoiceSprite;
   
    ArrayList listUserChoice;
    public TabletManager tabletManager;
    private string answer = "include, main, scanf, printf, return";
    public string puzzleID;
    public RoomID roomID;
    void Start()
    {
        listUserChoice = new ArrayList();
        RoomProgressManager.Instance.RegisterPuzzle(roomID, puzzleID);
    }


    public void Solve()
    {
        RoomProgressManager.Instance.MarkPuzzleFinished(roomID, puzzleID);
        dialogueSystem.StartDialogue(RoomID.Room1, DialogueID.Success);
    }

    public void addChoice(TextMeshProUGUI choice)
    {
        //Debug.Log(choice.text);
        listUserChoice.Add(choice.text);
    }

    public void checkResult()
    {
        //Debug.Log(string.Join(", ", listUserChoice.ToArray()));
        if (string.Join(", ", listUserChoice.ToArray()).Equals(answer))
        {
            Solve();
            tabletManager.greetingText.text = "Hello, " + tabletManager.getUsername();
            tabletManager.setStatusPuzzleInterface (false);
            tabletManager.setStatusSuccessInterface(true);
        }
        else
        {
            tabletManager.setStatusPuzzleInterface (false);
            tabletManager.setStatusErrorInterface(true);
        }
    }

    public void resetChoice()
    {

        foreach (GameObject item in listChoiceButton)
        {
            item.GetComponent<Image>().sprite = defaultChoiceSprite;
        }
        listUserChoice.Clear();
    }
}
