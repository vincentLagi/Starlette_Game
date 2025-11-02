using UnityEngine;

public enum GameState { FreeRoam, Interacting, OnTablet, OnPuzzle }
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] OxygenBar oxygenBar;
    [SerializeField] TabletManager tabletManager;
    GameState state;
    private void Start()
    {
        
        DialogManager.Instance.OnShowDialog += () => {
            state = GameState.Interacting;
            //Debug.Log("Interacting");
        };
        DialogManager.Instance.OnHideDialog += () => {
            if (state == GameState.Interacting){
                state = GameState.FreeRoam;
                //Debug.Log("FreeRoam");
            }
        };
    }
    public void  SetState(string newState)
    {
        if (System.Enum.TryParse(newState, true, out GameState parsedState))
        {
            state = parsedState;
            //Debug.Log($"State changed to: {state}");
        }
        //Debug.Log("State changed to: " + newState);
    }
    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerMovement.StartMove();
            playerMovement.HandleUpdate();
            oxygenBar.HandleUpdate();
        }
        else if (state == GameState.Interacting)
        {
            DialogManager.Instance.HandleUpdate();
            playerMovement.StopMove();
        }
        else if (state == GameState.OnTablet)
        {
            tabletManager.HandleUpdate();
            playerMovement.StopMove();
        }
        else if (state == GameState.OnPuzzle)
        {
            playerMovement.StopMove();
        }
    }
}
