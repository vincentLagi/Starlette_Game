using TMPro;
using UnityEngine;

public class SuccessErrorManagerScreen : MonoBehaviour
{

    [SerializeField] GameObject errorScreen;
    [SerializeField] GameObject successScreen;
    [SerializeField] GameController gameController;
    GameObject currentScreen;
    public void SetStatusErrorScreen(bool stat, string text = "")
    {
        errorScreen.SetActive(stat);
        errorScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        currentScreen.SetActive(!stat);
    }

    public void CloseErrorScreen()
    {
        errorScreen.SetActive(false);
        currentScreen.SetActive(true);
    }

    public void SetStatusSuccesScreen(bool stat)
    {
        successScreen.SetActive(stat);
        if (stat == false)
        {
            gameController.SetState("Freeroam");
        }
        else
        {
            currentScreen.SetActive(!stat);
        }
    }



    public void SetCurrentScreen(GameObject currScreen)
    {
        currentScreen = currScreen;
    }
}
