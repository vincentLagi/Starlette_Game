using UnityEngine;
using UnityEngine.UI;

public class ControlEnergyMonitorTask : MonoBehaviour
{

    GameObject selectedDoor;
    string chargedDoor;
    public GameObject doorCapsuleRoomUI;
    public GameObject door6UI;
    public Sprite selectedStatus;
    public Sprite unSelectedStatus;
    public GameObject doorCapsuleRoom;
    public GameObject doorRoom6;

    public bool isCharged = false;

    [SerializeField] ForPuzzleTask forPuzzleTask;
    [SerializeField] WhilePuzzleTask whilePuzzleTask;
    void Start()
    {
        RoomProgressManager.Instance.RegisterPuzzle(RoomID.Room7, "Decision1");
        RoomProgressManager.Instance.RegisterPuzzle(RoomID.Room6, "Decision2");
    }

    // Update is called once per frame
    void Update()
    {
        if (chargedDoor == "DoorToCapsule" && !isCharged)
        {
            doorCapsuleRoomUI.transform.Find("DoorStat").gameObject.GetComponent<Image>().sprite = selectedStatus;
            door6UI.transform.Find("DoorStat").gameObject.GetComponent<Image>().sprite = unSelectedStatus;

            if (forPuzzleTask.GetIsDone() && whilePuzzleTask.GetIsDone())
            {
                ControlDoorsActiveGameObject(RoomID.Room7, "Decision1");
            }
        }
        else if (chargedDoor == "Door6" && !isCharged)
        {
            doorCapsuleRoomUI.transform.Find("DoorStat").gameObject.GetComponent<Image>().sprite = unSelectedStatus;
            door6UI.transform.Find("DoorStat").gameObject.GetComponent<Image>().sprite = selectedStatus;

            if (forPuzzleTask.GetIsDone() && whilePuzzleTask.GetIsDone())
            {
                ControlDoorsActiveGameObject(RoomID.Room6, "Decision2");
            }
        }
    }

    void ControlDoorsActiveGameObject(RoomID roomID, string decision)
    {
        RoomProgressManager.Instance.MarkPuzzleFinished(roomID, decision);
        RoomProgressManager.Instance.MarkPuzzleFinished(RoomID.Room5, "Puzzle3");
        isCharged = true;
    }
    
    public void ChargedDoor()
    {
        chargedDoor = selectedDoor.name;

    }

    public void SelectedDoor(GameObject gameObject)
    {
        gameObject.GetComponent<Image>().color = Color.white;
        selectedDoor = gameObject;
    }

    public void setColorDoor(GameObject gameObject)
    {
         if (ColorUtility.TryParseHtmlString("#878787", out Color newColor))
    {
        gameObject.GetComponent<Image>().color = newColor;
    }
    }
}
