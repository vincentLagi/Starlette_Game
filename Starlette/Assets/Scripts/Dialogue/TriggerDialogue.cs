using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    private LayerMask playerLayer;
    [SerializeField] private DialogueSystem dialogueSystem;
    [SerializeField] private RoomID selectedRoom;
    [SerializeField] private DialogueID selectedDialogue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("Trigger Interact.");
            dialogueSystem.StartDialogue(selectedRoom, selectedDialogue);
            Destroy(gameObject);
        }
    }
}
