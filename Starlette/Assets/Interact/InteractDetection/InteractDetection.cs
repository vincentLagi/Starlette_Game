using UnityEngine;

public class InteractDetection : MonoBehaviour
{
    private LayerMask playerLayer; 
    public GameObject interactIcon;

    void Start()
    {
        interactIcon.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       //Debug.Log(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("Player entered interaction zone.");
            interactIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting object is on the Player layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("Player left interaction zone.");
            interactIcon.SetActive(false);
        }
    }
}