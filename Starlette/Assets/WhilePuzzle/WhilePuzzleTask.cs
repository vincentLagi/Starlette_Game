using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class WhilePuzzleTask : MonoBehaviour
{

    int packet = 80;
    int j;
    int rangeLoop;
    int jm;
    int packetRm;
    int op;
    string packetOp;
    string jOp;
    public GameObject valueBlock;
    public GameObject operatorBlock;
    private List<GameObject> listBlockMenu = new List<GameObject>();
    public WhileChoiceBlockMenu choiceBlockMenu;
    public TextMeshProUGUI totalPaketText;
    public TextMeshProUGUI conditionalOpText;

    public WhilePuzzleEmptyBlock whilePuzzleEmptyBlock;
    public GameObject whilePuzzleScreen;
    public SuccessErrorManagerScreen successErrorManagerScreen;
    public GameObject sendingPacketScreen;
    private bool isDone = false;
    private struct UserChoice
    {
        public int X;
        public int XRangeLoop;
        public string PacketOp;
        public int XPacketRm;
        public string XOp;
        public int XRm;
    }
    public string puzzleID;
    public RoomID roomID;

    void Start()
    {
        RoomProgressManager.Instance.RegisterPuzzle(roomID, puzzleID);
        GenerateAnswer();
        GenerateBlock();
        
        //Debug.Log(packet);
    }

    public void Solve()
    {
        RoomProgressManager.Instance.MarkPuzzleFinished(roomID, puzzleID);
    }

    public async void RunCode()
    {
        successErrorManagerScreen.SetCurrentScreen(whilePuzzleScreen);
        if (!ValidateBlockCount())
        {
            successErrorManagerScreen.SetStatusErrorScreen(true, "Cannot Empty");
            return;
        }

        if (!GetUserAnswers(out var values))
        {
            successErrorManagerScreen.SetStatusErrorScreen(true, "Invalid input values");
            return;
        }

        whilePuzzleScreen.SetActive(false);
        sendingPacketScreen.SetActive(true);
        sendingPacketScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = packet.ToString();

        int result = await ProcessLoop(values);

        if (result == -1)
        {
            sendingPacketScreen.SetActive(false);
            successErrorManagerScreen.SetStatusErrorScreen(true, "Your Code Makes The System Sending Wrong Amount of Packets");
        }
        else
        {
            if (result == packet)
            {
                sendingPacketScreen.SetActive(false);
                successErrorManagerScreen.SetStatusSuccesScreen(true);
                Solve();
                isDone = true;

            }
            else
            {
                sendingPacketScreen.SetActive(false);
                successErrorManagerScreen.SetStatusErrorScreen(true, "Your Code Makes The System Sending Wrong Amount of Packets");
            }
        }
    }
    bool ValidateBlockCount()
    {
        return whilePuzzleEmptyBlock.GetLengthListBlockMenu() == 6;
    }

    bool GetUserAnswers(out UserChoice values)
    {
        values = new UserChoice();

        try
        {
            GameObject[] valueBlocks = whilePuzzleEmptyBlock.GetListEmptyValueBlock();
            GameObject[] operatorBlocks = whilePuzzleEmptyBlock.GetListEmptyOperatorBlock();

            values.X = GetIntFromText(valueBlocks[0]);
            values.XRangeLoop = GetIntFromText(valueBlocks[1]);
            values.PacketOp = GetTextFromComponent(operatorBlocks[0]);
            values.XPacketRm = GetIntFromText(valueBlocks[2]);
            values.XOp = GetTextFromComponent(operatorBlocks[1]);
            values.XRm = GetIntFromText(valueBlocks[3]);

            return true;
        }
        catch
        {
            return false;
        }
    }

    async Task<int> ProcessLoop(UserChoice values)
    {
        int x = values.X;
        int xPacket = 80;
        int maxLoop = 16;
        int currentLoop = 0;
        bool isGreaterThan = conditionalOpText.text == ">";

        while (isGreaterThan ? x > values.XRangeLoop : x < values.XRangeLoop)
        {
            xPacket = values.PacketOp == "+" ? xPacket + values.XPacketRm : xPacket - values.XPacketRm;

            x = isGreaterThan ? x - values.XRm : x + values.XRm;

            sendingPacketScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = xPacket.ToString();

            currentLoop++;
            if (currentLoop >= maxLoop)
            {
                return -1;
            }

            await Task.Delay(2000);
        }

        if (currentLoop == 0)
        {
            return -1;
        }
        return xPacket;
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


    void GenerateAnswer()
    {
        RandomAllVarible();
        if (generateRandomInteger(1, 3) == 1)
        {
            // > -
            rangeLoop = generateRandomInteger(j - 15, j - 1);
            conditionalOpText.text = ">";
            while (j > rangeLoop)
            {
                if (op == 1)
                {
                    packet += packetRm;
                    packetOp = "+";
                }
                else
                {
                    packet -= packetRm;
                    packetOp = "-";
                }

                jOp = "-";
                j -= jm;
            }
        }
        else
        {
            // < +
            rangeLoop = generateRandomInteger(j + 1, j + 15);
            conditionalOpText.text = "<";
            while (j < rangeLoop)
            {
                if (op == 1)
                {
                    packet += packetRm;
                    packetOp = "+";
                }
                else
                {
                    packet -= packetRm;
                    packetOp = "-";
                }
                jOp = "+";
                j += jm;
            }
        }

    }

    void GenerateBlock()
    {


        choiceBlockMenu.SpawnNewObject(valueBlock, rangeLoop.ToString());
        choiceBlockMenu.SpawnNewObject(valueBlock, packetRm.ToString());
        choiceBlockMenu.SpawnNewObject(valueBlock, jm.ToString());
        choiceBlockMenu.SpawnNewObject(operatorBlock, packetOp.ToString());
        choiceBlockMenu.SpawnNewObject(operatorBlock, jOp.ToString());

        totalPaketText.text = packet.ToString();
    }

    void RandomAllVarible()
    {
        j = generateRandomInteger();

        choiceBlockMenu.SpawnNewObject(valueBlock, j.ToString());

        op = generateRandomInteger(1, 3);
        jm = generateRandomInteger(1, 4);
        packetRm = generateRandomInteger(1, 4);
    }

    int generateRandomInteger(int min = 1, int max = 16)
    {
        return Random.Range(min, max);
    }

    public bool GetIsDone()
    {
        return isDone;
    }
}
