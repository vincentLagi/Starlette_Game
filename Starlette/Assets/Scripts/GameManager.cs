using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameUI_Manager gameUI_Manager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            gameUI_Manager.TogglePauseUI();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1; // Gamenya jalan (gk di pause)
        SceneManager.LoadScene("MainMenu");
    }
}
