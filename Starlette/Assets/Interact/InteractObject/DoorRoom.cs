using UnityEngine;

public class DoorRoom : MonoBehaviour, Interactable
{
    public GameObject spawnPoint;
    public GameObject character;
    public RoomID currentRoom;
    public DialogueID errorDialogueMessage;
    [SerializeField] private DialogueSystem dialogueSystem;

    private void Awake()
    {
        dialogueSystem = GameObject.Find("DialogueObject").GetComponent<DialogueSystem>();
    }

    public void Interact()
    {
        var roomData = RoomProgressManager.Instance.GetRoomData(currentRoom);

        if (roomData.progress >= 100f)
        {
            character.transform.position = spawnPoint.transform.position;
            //if (roomData.timeSpent == 0)
            //{
            //    RoomProgressManager.Instance.EndRoom(roomData.roomID);
            //}
            //Debug.Log("Move to next room.");
            if(currentRoom == RoomID.Room7)
            {
                GameObject parent = GameObject.Find("Cutscene");
                
                parent.transform.GetChild(1).gameObject.SetActive(true);
                RoomProgressManager.Instance.OnGameFinished();
            }

            if (currentRoom == RoomID.Room6)
            {
                RoomProgressManager.Instance.OnGameFinished();
            }
        }
        else
        {
            //Debug.Log($"Cant move to next room. Progress {roomData.progress}%");
            //Nanti disini kita panggil dialogue
            dialogueSystem.StartDialogue(currentRoom, errorDialogueMessage);
        }
    }
}
