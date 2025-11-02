using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ForPuzzleTask : MonoBehaviour
{
    int packet;
    private Answer answer;
    private Answer userChoice;
    public GameObject valueBlock;
    public GameObject operatorBlock;
    public ForChoiceBlockMenu forChoiceBlockMenu;
    public ForPuzzleEmptyBlock forPuzzleEmptyBlock;
    public TextMeshProUGUI totalPacketText;
    public TextMeshProUGUI conditionalOpText;
    public SuccessErrorManagerScreen successErrorManagerScreen;
    public GameObject forPuzzleScreen;
    public GameObject sendingPacketScreen;
    private bool isDone = false;
    private struct Answer
    {
        public int iValue;
        public int iValidation;
        public int iIncDec;
        public string iOp;
        public string packetOp;
        public int packetIncDec;
    }
    public string puzzleID;
    public RoomID roomID;
    void Start()
    {
        RoomProgressManager.Instance.RegisterPuzzle(roomID, puzzleID);
        answer = new Answer();
        GenerateAnswer();
        GenerateBlock();
    }

    public void Solve()
    {
        RoomProgressManager.Instance.MarkPuzzleFinished(roomID, puzzleID);
    }
    public async void RunCode()
    {
        successErrorManagerScreen.SetCurrentScreen(forPuzzleScreen);
        if (!ValidationLengthChoice())
        {
            successErrorManagerScreen.SetStatusErrorScreen(true, "Cannot Empty");
        }

        GetUserChoice();
        sendingPacketScreen.SetActive(true);
        sendingPacketScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = totalPacketText.text;
        forPuzzleScreen.SetActive(false);

        int result = await ProcessLoop();
        Debug.Log(result);

        if (result == -1)
        {
            sendingPacketScreen.SetActive(false);
            successErrorManagerScreen.SetStatusErrorScreen(true, "Your Code Makes The System Sending Wrong Amount of Packets");
        }
        else
        {
            if (result == packet)
            {
                Solve();
                sendingPacketScreen.SetActive(false);
                forPuzzleScreen.SetActive(false);
                successErrorManagerScreen.SetStatusSuccesScreen(true);
                isDone = true;

            }
            else
            {
                sendingPacketScreen.SetActive(false);
                successErrorManagerScreen.SetStatusErrorScreen(true, "Your Code Makes The System Sending Wrong Amount of Packets");
            }
        }
    }

    async Task<int> ProcessLoop()
{
    int iPacket = 100;
    int maxLoop = 15;
    int currLoop = 0;
    bool isLessThan = conditionalOpText.text == "<";
    int increment = isLessThan ? userChoice.iIncDec : -userChoice.iIncDec;
    
    UpdatePacketDisplay(iPacket);
    await Task.Delay(1000);

    for (int i = userChoice.iValue;
        isLessThan ? i < userChoice.iValidation : i > userChoice.iValidation;
        i += increment)
    {
        UpdatePacketDisplay(iPacket);
        await Task.Delay(1000);

        iPacket = userChoice.packetOp == "+"
            ? iPacket + userChoice.packetIncDec
            : iPacket - userChoice.packetIncDec;

        currLoop++;
        if (currLoop >= maxLoop) return -1;
    }

    UpdatePacketDisplay(iPacket);
    await Task.Delay(1000);

    if (currLoop <= 0) return -1;
    return iPacket;
}

void UpdatePacketDisplay(int value)
{
    sendingPacketScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = value.ToString();
}

    void GetUserChoice()
    {
        userChoice = new Answer();
        GameObject[] valueBlocks = forPuzzleEmptyBlock.GetListEmptyValueBlock();
        GameObject[] operatorBlocks = forPuzzleEmptyBlock.GetListEmptyOperatorBlock();

        userChoice.iValue = GetIntFromText(valueBlocks[0]);
        userChoice.iValidation = GetIntFromText(valueBlocks[1]);
        userChoice.iIncDec = GetIntFromText(valueBlocks[2]);
        userChoice.packetIncDec = GetIntFromText(valueBlocks[3]);
        userChoice.iOp = GetTextFromComponent(operatorBlocks[0]);
        userChoice.packetOp = GetTextFromComponent(operatorBlocks[1]);

        
    }

    int GetIntFromText(GameObject parent)
    {
        string text = GetTextFromComponent(parent);
        return int.Parse(text);
    }

    string GetTextFromComponent(GameObject parent)
    {
        return parent.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }

    bool ValidationLengthChoice()
    {
        return forPuzzleEmptyBlock.GetLengthListBlockMenu() == 6;
    }
    void GenerateAnswer()
    {
        RandomAllValueAnswer();
        packet = 100;
        if (GenerateRandomInteger(1, 3) == 1)
        {
            // >
            answer.iValidation = GenerateRandomInteger(answer.iValue + 1, answer.iValue + 10);
            answer.iOp = "+";

            int packetOp = GenerateRandomInteger(1, 3);
            conditionalOpText.text = "<";

            for (int i = answer.iValue; i < answer.iValidation; i += answer.iIncDec)
            {
                if (packetOp == 1)
                {
                    answer.packetOp = "+";
                    packet += answer.packetIncDec;
                }
                else
                {
                    answer.packetOp = "-";
                    packet -= answer.packetIncDec;
                }
            }
        }
        else
        {
            answer.iValidation = GenerateRandomInteger(answer.iValue - 10, answer.iValue - 1);
            answer.iOp = "-";

            int packetOp = GenerateRandomInteger(1, 3);
            conditionalOpText.text = ">";

            for (int i = answer.iValue; i > answer.iValidation; i -= answer.iIncDec)
            {
                if (packetOp == 1)
                {
                    answer.packetOp = "+";
                    packet += answer.packetIncDec;
                }
                else
                {
                    answer.packetOp = "-";
                    packet -= answer.packetIncDec;
                }
            }
        }

        totalPacketText.text = packet.ToString();
    }

    void GenerateBlock()
    {
        forChoiceBlockMenu.SpawnNewObject(valueBlock, answer.iValue.ToString());
        forChoiceBlockMenu.SpawnNewObject(valueBlock, answer.iValidation.ToString());
        forChoiceBlockMenu.SpawnNewObject(valueBlock, answer.iIncDec.ToString());
        forChoiceBlockMenu.SpawnNewObject(valueBlock, answer.packetIncDec.ToString());
        forChoiceBlockMenu.SpawnNewObject(operatorBlock, answer.iOp);
        //Debug.Log(answer.packetOp);
        forChoiceBlockMenu.SpawnNewObject(operatorBlock, answer.packetOp);


    }

    void RandomAllValueAnswer()
    {
        answer.iValue = GenerateRandomInteger();
        answer.iIncDec = GenerateRandomInteger(1, 4);
        answer.packetIncDec = GenerateRandomInteger(1, 4);
    }

    int GenerateRandomInteger(int min = 1, int max = 16)
    {
        return Random.Range(min, max);
    }

    public bool GetIsDone()
    {
        return isDone;
    }
}
