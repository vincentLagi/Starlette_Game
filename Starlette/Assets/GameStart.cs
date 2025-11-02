using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject openingScene;

    public void startGame()
    {
        Debug.Log("Game started!");
        RoomProgressManager.Instance.StartGameplayTimer();
        openingScene.SetActive(false);
        player.SetActive(true);
    }
}
