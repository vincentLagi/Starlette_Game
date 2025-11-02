using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IfElseTask : MonoBehaviour
{
    
    public Color[] listColor;
    public Image[] listPattern;
    public List<Color> listResultColor = new List<Color>();
    public TextMeshProUGUI[] listSpeedText;
    private System.Random random = new System.Random();
    public GameObject[] blockButton;
    public TextMeshProUGUI codeText;
    private HashSet<int> _usedNumbers = new HashSet<int>();
    private string ope;
    private int index;
    public IfElseSlider[] sliders;
    private bool isComplete = false;
    public SuccessErrorManagerScreen successErrorManagerScreen;
    public GameController gameController;

    public string puzzleID;
    public RoomID roomID;
    void Start()
    {
        RoomProgressManager.Instance.RegisterPuzzle(roomID, puzzleID);
        ResetTask();
        StartCoroutine(CheckCompletion());
    }
     public void Solve()
    {
        
        RoomProgressManager.Instance.MarkPuzzleFinished(roomID, puzzleID);
    }
    private float GenerateRandomFloat()
    {
        return Mathf.Round(Random.Range(0.1f, 0.15f) * 100f) / 100f;
    }

    private int GenerateRandomInt()
    {

        int randomNumber;
        do
        {
            randomNumber = Random.Range(0, 100);
        } while (_usedNumbers.Contains(randomNumber));
        _usedNumbers.Add(randomNumber);
        return randomNumber;
    }
    void AssignUniqueRandomColors()
    {

        for (int i = 0; i < listColor.Length; i++)
        {
            int randomIndex = Random.Range(i, listColor.Length);
            Color temp = listColor[i];
            listColor[i] = listColor[randomIndex];
            listColor[randomIndex] = temp;
        }

        for (int i = 0; i < listPattern.Length; i++)
        {
            if (listPattern[i] != null)
            {
                listPattern[i].color = listColor[i];
            }
        }
    }
    void AssignRandomSpeed()
    {
        for (int i = 0; i < 3; i++)
        {
            float val = GenerateRandomFloat();
            listSpeedText[i].text = val.ToString();
            sliders[i].SetSpeed(val);
        }
    }

    void AssignRandomBlock()
    {
        for (int i = 0; i < 3; i++)
        {
            blockButton[i].GetComponentInChildren<TextMeshProUGUI>().text = GenerateRandomInt().ToString();
        }
    }

    public void ChangeOperator(TextMeshProUGUI op)
    {
        if (op.text == "+")
        {
            op.text = "-";
            ope = "-";
        }
        else
        {
            op.text = "+";
            ope = "+";
        }
        SetCodeText();
    }


    public void ChangeSection(TextMeshProUGUI text)
    {
        string section = text.text;
        if (section == blockButton[0].GetComponentInChildren<TextMeshProUGUI>().text)
        {
            float value;
            float.TryParse(listSpeedText[0].text, out value);
            if (ope == "+")
            {
                value += 0.01f;
            }
            else
            {
                value -= 0.01f;
            }
            listSpeedText[0].text = value.ToString();
        }
        else if (section == blockButton[index].GetComponentInChildren<TextMeshProUGUI>().text)
        {
            float value;
            float.TryParse(listSpeedText[1].text, out value);
            if (ope == "+")
            {
                value += 0.01f;
            }
            else
            {
                value -= 0.01f;
            }
            listSpeedText[1].text = value.ToString();
        }
        else
        {
            float value;
            float.TryParse(listSpeedText[2].text, out value);
            if (ope == "+")
            {
                value += 0.01f;
            }
            else
            {
                value -= 0.01f;
            }
            listSpeedText[2].text = value.ToString();
        }
    }

    public int ToInt(string str, int defaultValue)
    {
        return int.TryParse(str, out int result) ? result : defaultValue;
    }

    void SetCodeText()
    {
        string kalimat =
$@"int section;
if (section == {blockButton[0].GetComponentInChildren<TextMeshProUGUI>().text}) {{
    redSpeed {ope}= 0.01f;
}}else if (section == {blockButton[index].GetComponentInChildren<TextMeshProUGUI>().text}) {{
    blueSpeed {ope}= 0.01f; 
}}else {{
    greenSpeed {ope}= 0.01f;
}}";
        codeText.text = kalimat;
    }

    public void Fill()
    {
        successErrorManagerScreen.SetCurrentScreen(this.gameObject.transform.GetChild(0).gameObject);
        sliders[0].StartFill();
        sliders[1].StartFill();
        sliders[2].StartFill();

    }

    public void AddResultColor(Color color)
    {
        listResultColor.Add(color);
    }

    private IEnumerator CheckCompletion()
    {
        while (!isComplete)
        {
            int successCount = 0;
            if (listResultColor.Count >= 3)
            {

                for (int i = 0; i < 3; i++)
                {
                    if (listPattern[i].color == listResultColor[i]) { successCount++; }

                }
            }

            if (listResultColor.Count >= 3)
            {
                if (successCount >= 3)
                {
                    this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    successErrorManagerScreen.SetStatusSuccesScreen(true);
                    Solve();
                    isComplete = true;
                    Debug.Log(isComplete);
                }
                else
                {
                    successErrorManagerScreen.SetStatusErrorScreen(true, "You Need To Adjust The Speed !!!");
                    listResultColor.Clear();
                    ResetTask();
                }
            }
            else
            {
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    void ResetTask()
    {
        AssignUniqueRandomColors();
        AssignRandomSpeed();
        AssignRandomBlock();
        ope = "+";
        index = Random.Range(1, 3);
        sliders[0].image.fillAmount = 0f;
        sliders[1].image.fillAmount = 0f;
        sliders[2].image.fillAmount = 0f;
        blockButton[3].GetComponentInChildren<TextMeshProUGUI>().text = ope;
        isComplete = false;
        listResultColor.Clear();
        SetCodeText();
    }

    public void SetFindHiddenPuzzle()
    {
        ResetTask();
        Debug.Log(isComplete);
        StartCoroutine(CheckCompletion());
    }

    public bool getIsComplete()
    {
        return isComplete;
    }
}
