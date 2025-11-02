using UnityEngine;

public class Check2Monitor : MonoBehaviour
{
    public LeftComputerTask leftComputerTask;
    public RightComputerTask rightComputerTask;
    private bool isShowHiddenPuzzle = false;
    private bool isFinishHiddenPuzzle = false;
    public GameObject specialDoor;
    public GameObject hiddenPuzzle;
    void Start()
    {

    }

    void Update()
    {

        if (!isShowHiddenPuzzle && leftComputerTask.getIsComplete() && rightComputerTask.getIsComplete())
        {

            specialDoor.layer = LayerMask.NameToLayer("Interactable");
            specialDoor.transform.GetChild(0).gameObject.SetActive(true);
            hiddenPuzzle.layer = LayerMask.NameToLayer("Interactable");
            hiddenPuzzle.transform.GetChild(0).gameObject.SetActive(true);
            // Debug.Log(isShowHiddenPuzzle);
        }
        else if (isShowHiddenPuzzle)
        {
            if (!isFinishHiddenPuzzle && leftComputerTask.getIsComplete() && rightComputerTask.getIsComplete())
            {
                isFinishHiddenPuzzle = true;
            }
        }
    }

    public void ResetPuzzle()
    {
        leftComputerTask.SetFindHiddenPuzzle();
        rightComputerTask.SetFindHiddenPuzzle();
    }

    public void SetIsShowHiddenPuzzle(bool b)
    {
        isShowHiddenPuzzle = b;
    }

    public bool GetIsFinishHiddenPuzzle()
    {
        return isFinishHiddenPuzzle;
    }
}
