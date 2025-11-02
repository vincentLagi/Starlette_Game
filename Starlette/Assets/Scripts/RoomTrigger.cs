using UnityEngine;
using System.Collections.Generic;

public class RoomTrigger : MonoBehaviour
{
    public RoomID roomID;
    public List<string> puzzleIDsInThisRoom;

    private bool hasRegistered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasRegistered && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("Entered room: " + roomID);
            foreach (string puzzleID in puzzleIDsInThisRoom)
            {
                RoomProgressManager.Instance.RegisterPuzzle(roomID, puzzleID);
            }

            //RoomProgressManager.Instance.StartRoom(roomID);
            hasRegistered = true;
        }
    }
}
