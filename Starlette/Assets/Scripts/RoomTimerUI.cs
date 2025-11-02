using UnityEngine;
using TMPro;

public class RoomTimerUI : MonoBehaviour
{
    public RoomID currentRoom;
    public TextMeshProUGUI timerText;

    void Update()
    {
        var data = RoomProgressManager.Instance.GetRoomData(currentRoom);
        float timeInRoom = data.timeSpent;

        if (data.startTime > 0)
        {
            timeInRoom += Time.time - data.startTime;
        }

        timerText.text = FormatTime(timeInRoom);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
