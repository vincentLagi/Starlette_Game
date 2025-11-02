using UnityEngine;

public class UserTeleporter : MonoBehaviour, Interactable
{
    [SerializeField] private GameObject NextSpawnPoint;
    public void Interact()
    {
        // Implement the interaction logic here
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = NextSpawnPoint.transform.position;

            Vector2 faceDirection = Vector2.up;
            player.GetComponent<PlayerMovement>().SetFacingDirection(faceDirection);
            Debug.Log("Interacting with UserTeleporter");
        }
        else
        {
            Debug.LogWarning("Player not found");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
