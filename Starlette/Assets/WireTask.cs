using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WireTask : MonoBehaviour
{

    public GameObject[] leftWireText;
    public GameObject[] rightWireText;

    private ArrayList listDataType = new ArrayList { "string", "int", "float", "bool", "char" };
    public Wire currentDraggedWire;
    public Wire currentHoveredWire;
    private System.Random random = new System.Random();
    private string[] leftWireDataTypes = new string[4];
    private object[] rightWireValues = new object[4];
    public bool isComplete = false;
    public GameObject wirePuzzleInterface;
    public GameObject interactRange;
    // public GameObject doorInteractRange;
    public GameObject monitor;
    public GameObject toolBag;
    public GameController gameController;
    public string puzzleID;
    public RoomID roomID;
    private IEnumerator CheckCompletion()
    {
        while (!isComplete)
        {
            int successCount = 0;
            for (int i = 0; i < rightWireText.Length; i++)
            {
                if (rightWireText[i].GetComponentInParent<Wire>().isSuccess) { successCount++; }

            }

            if (successCount >= 4)
            {
                toolBag.SetActive(true);
                interactRange.SetActive(false);
                // doorInteractRange.SetActive(true);
                setStatusWirePuzzleInterface(false);
                gameController.SetState("FreeRoam");
                monitor.GetComponent<DoorMonitorRoomOne>().enabled = false;
                monitor.layer = LayerMask.NameToLayer("UI");
                Solve();
            }
            else
            {
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    void Start()
    {
        RoomProgressManager.Instance.RegisterPuzzle(roomID, puzzleID);
        ArrayList shuffledTypes = new ArrayList(listDataType);
        ShuffleArrayList(shuffledTypes);

        for (int i = 0; i < 4; i++)
        {
            leftWireDataTypes[i] = (string)shuffledTypes[i];
            leftWireText[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = leftWireDataTypes[i];
            leftWireText[i].GetComponentInParent<Wire>().setDataType(leftWireDataTypes[i]);
        }

        for (int i = 0; i < 4; i++)
        {
            rightWireValues[i] = GenerateRandomValue(leftWireDataTypes[i]);
        }
        ShuffleGameObjectArray(rightWireText);

        for (int i = 0; i < 4; i++)
        {
            rightWireText[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = rightWireValues[i].ToString();
        }
        StartCoroutine(CheckCompletion());
    }
    


    public void Solve()
    {
        RoomProgressManager.Instance.MarkPuzzleFinished(roomID, puzzleID);
    }

    private void ShuffleArrayList(ArrayList list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            object temp = list[i];
            int randomIndex = random.Next(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    private void ShuffleGameObjectArray(GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = random.Next(i, array.Length);
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
    public object GenerateRandomValue(string dataType)
    {
        switch (dataType.ToLower())
        {
            case "string": return GenerateRandomString();
            case "int": return GenerateRandomInt();
            case "float": return GenerateRandomFloat();
            case "bool": return GenerateRandomBool();
            case "char": return GenerateRandomChar();
            default: throw new ArgumentException($"Unknown data type: {dataType}");
        }
    }
    private string GenerateRandomString()
    {
        string[] randomStrings = { "Hello", "World", "Unity", "CSharp", "Game", "Dev", "Random", "Text" };
        return randomStrings[random.Next(randomStrings.Length)];
    }

    private int GenerateRandomInt()
    {
        return random.Next(-100, 101);
    }

    private float GenerateRandomFloat()
    {
        float value = (float)(random.NextDouble() * 200f - 100f);
        return Mathf.Round(value * 100f) / 100f;
    }

    private bool GenerateRandomBool()
    {
        return random.Next(2) == 1;
    }

    private char GenerateRandomChar()
    {
        return (char)random.Next('A', 'Z' + 1);
    }

    public object getValueByDataType(string dataType)
    {
        int value = Array.IndexOf(leftWireDataTypes, dataType);
        
        return rightWireValues[value];
    }
    public void setStatusWirePuzzleInterface(bool b)
    {
        wirePuzzleInterface.SetActive(b);
    }
}
