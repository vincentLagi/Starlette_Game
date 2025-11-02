using UnityEngine;

public class TestingCompletePuzzle : MonoBehaviour
{
    public string puzzleID;
    public RoomID roomID;

    void Start()
    {
        RoomProgressManager.Instance.RegisterPuzzle(roomID, puzzleID);
    }

    public void Solve()
    {
        RoomProgressManager.Instance.MarkPuzzleFinished(roomID, puzzleID);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Puzzle " + puzzleID + " in room " + roomID + " is being solved.");
            Solve();
        }
    }
}
