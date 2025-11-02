using UnityEngine;

public class GameUI_Manager : MonoBehaviour
{
    GameManager GM;
    public GameObject UI_Pause;

    private enum GameUI_State
    {
        GamePlay, Pause
    }

    GameUI_State currentState;

    void Start()
    {
        SwitchUIState(GameUI_State.GamePlay);
    }

    private void SwitchUIState(GameUI_State state)
    {
        UI_Pause.SetActive(false);

        Time.timeScale = 1; // Gamenya jalan (gk di pause)
        switch (state)
        {
            case GameUI_State.GamePlay:
                break;
            case GameUI_State.Pause:
                Time.timeScale = 0; // Gamenya di pause
                UI_Pause.SetActive(true);
                break;
        }

        currentState = state;
    }

    public void TogglePauseUI()
    {
        if (currentState == GameUI_State.GamePlay)
            SwitchUIState(GameUI_State.Pause);
        else if (currentState == GameUI_State.Pause)
            SwitchUIState(GameUI_State.GamePlay);
    }
}
