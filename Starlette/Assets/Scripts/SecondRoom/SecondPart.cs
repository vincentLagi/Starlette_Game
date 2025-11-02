using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecondPart : MonoBehaviour
{
    [SerializeField] private GameObject secondPart;
    [SerializeField] private GameObject firstPart;
    [SerializeField] private BlockFactory blockFactory;
    [SerializeField] private DialogueSystem dialogueSystem;
    private CompilerContext context;
    public SuccessErrorManagerScreen successErrorManagerScreen;
    private bool isDone = false;
    public string puzzleID;
    public RoomID roomID;
    private void Start()
    {
        RoomProgressManager.Instance.RegisterPuzzle(roomID, puzzleID);
        context = GetComponentInParent<CompilerContext>();
        if (context == null)
        {
            Debug.LogError("CompilerContext not found in the parent.");
            return;
        }

        if (firstPart == null || secondPart == null)
        {
            Debug.LogError("First or Second part GameObject is not assigned.");
            return;
        }
        SetUpLeftPart();
        SetUpRightPart();
    }
    public void Solve()
    {
        isDone = true;
        RoomProgressManager.Instance.MarkPuzzleFinished(roomID, puzzleID);
        dialogueSystem.StartDialogue(RoomID.Room2, DialogueID.Success);
    }

    public bool GetIsDone()
    {
        return isDone;
    }

    private void SetUpRightPart()
    {
        FreeBlockContainer sequences = secondPart.GetComponentInChildren<FreeBlockContainer>();
        SetUpSequences(sequences);
    }

    private void SetUpLeftPart()
    {
        // randomize 2 literal block at above
        SetUpLiteralBlock();
        SetUpOperatorBlock();
    }   

    private void SetUpLiteralBlock()
    {
        LiteralBlock[] intBlocks = firstPart.GetComponentsInChildren<LiteralBlock>();
        SetUpTopBlockListener(intBlocks[0]);
        SetUpTopBlockListener(intBlocks[1]);
    }

    private void SetUpTopBlockListener(LiteralBlock block)
    {
        // add listener to block
        block.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        block.gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            // when clicked, transfer to the holder.
            if (block.GetComponentInParent<BlockHolder>() == null)
            {
                // state pindahin ke BlockHolder
                GameObject cloneBlock = blockFactory.CreateBlock(BlockType.Literal_Int, block.GetValue(), firstPart.transform);
                cloneBlock.GetComponent<Button>().onClick.AddListener(() =>
                {
                    // state pindahin ke BlockHolder
                    cloneBlock.GetComponent<Button>().onClick.RemoveAllListeners();
                    cloneBlock.GetComponentInParent<BlockHolder>().RemoveBlock(cloneBlock);
                    Destroy(cloneBlock);
                });

                firstPart.GetComponentInChildren<BlockHolder>().AddBlock(cloneBlock);
            }
            
        });
        block.Init(Integer.GetRandomValue());
        block.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = block.ToString();
    }

    private void SetUpOperatorBlock()
    {
        FreeBlockContainer container = firstPart.GetComponentInChildren<FreeBlockContainer>();
        if (container == null)
        {
            Debug.LogError("FreeBlockContainer not found in the first part.");
            return;
        }

        SetParenthesisBlock(container);
        SetUpAritmethicBlock(container);
        SetUpComparisonBlock(container);
    }

    private void SetParenthesisBlock(FreeBlockContainer container)
    {
        float x = -180.0f + container.GetSpacing();
        foreach (ParenthesisType type in System.Enum.GetValues(typeof(ParenthesisType)))
        {
            ParenthesisBlock parenthesisBlock = blockFactory.CreateBlock(BlockType.Parenthesis, type, container.transform).GetComponent<ParenthesisBlock>();
            parenthesisBlock.Init(type);
            parenthesisBlock.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = parenthesisBlock.ToString();
            parenthesisBlock.gameObject.transform.localPosition = new Vector3(x, 40, 0);
            x += FreeBlockContainer.GetBlockWidth(parenthesisBlock.gameObject) + container.GetSpacing();
        }
    }

    private void SetUpAritmethicBlock(FreeBlockContainer container)
    {
        float x = -180.0f + container.GetSpacing();
        foreach (ArithmeticType type in System.Enum.GetValues(typeof(ArithmeticType)))
        {
            if (type == ArithmeticType.Random || type == ArithmeticType.RandomNoModulo) continue; // skip Random type   
            ArithmeticOperatorBlock arithmeticBlock = blockFactory.CreateBlock(BlockType.ArithmeticOperator, type, container.transform).GetComponent<ArithmeticOperatorBlock>();
            arithmeticBlock.Init(type);
            arithmeticBlock.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = arithmeticBlock.ToString();
            arithmeticBlock.gameObject.transform.localPosition = new Vector3(x, 0, 0);
            x += FreeBlockContainer.GetBlockWidth(arithmeticBlock.gameObject) + container.GetSpacing();
        }
    }

    private void SetUpComparisonBlock(FreeBlockContainer container)
    {
    
        float x = -180.0f + container.GetSpacing();
        foreach (ComparisonType type in System.Enum.GetValues(typeof(ComparisonType)))
        {
            if (type == ComparisonType.Random) continue; // skip Random type
            if (type == ComparisonType.LessEqual || type == ComparisonType.GreaterEqual || type == ComparisonType.NotEqual) continue; 
            ComparisonOperatorBlock comparisonBlock = blockFactory.CreateBlock(BlockType.ComparisonOperator, type, container.transform).GetComponent<ComparisonOperatorBlock>();
            comparisonBlock.Init(type);
            comparisonBlock.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = comparisonBlock.ToString();
            comparisonBlock.gameObject.transform.localPosition = new Vector3(x, -40, 0);
            x += FreeBlockContainer.GetBlockWidth(comparisonBlock.gameObject) + container.GetSpacing();
        }
    }

    public void ExecuteSequence(BaseBlockContainer holder)
    {
        successErrorManagerScreen.SetCurrentScreen(this.gameObject.transform.parent.gameObject);
        // Implementation for executing the code block
        if (holder == null)
        {
            // bukain panel nanti
            Debug.LogError("Holder is null, cannot execute sequence.");
            successErrorManagerScreen.SetStatusErrorScreen(true,"Holder is null, cannot execute sequence.");
            return;
        }
        List<GameObject> blocks = holder.GetAllBlocks();
        // Debug.Log($"Executing sequence with {blocks.Count} blocks.");
        VariableBlockAdapter variableBlock = firstPart.GetComponentInChildren<VariableBlockAdapter>();
        if (variableBlock == null)
        {
            Debug.LogError("First block is not a VariableBlock.");
            successErrorManagerScreen.SetStatusErrorScreen(true,"First block is not a VariableBlock.");
            return;
        }

        
        PayloadResultModel variableCheck = VariableBlockAdapter.IsValidVariableName(variableBlock.VariableName);
        if (variableCheck.Success == false)
        {
            // open panel
            Debug.LogError($"{variableCheck.Message}");
            successErrorManagerScreen.SetStatusErrorScreen(true,$"{variableCheck.Message}");
            return;
        }

        AssignmentOperatorBlock assignmentObject = firstPart.GetComponentInChildren<AssignmentOperatorBlock>();
        if (assignmentObject == null)
        {
            Debug.LogError("Assignment block is not of type AssignmentOperatorBlock.");
            successErrorManagerScreen.SetStatusErrorScreen(true,"Assignment block is not of type AssignmentOperatorBlock.");
            return;
        }

        List<CodeBlock> codeBlocks = new List<CodeBlock>();
        CodeBlock prev = null;
        foreach (GameObject block in blocks)
        {
            CodeBlock codeBlock = block.GetComponent<CodeBlock>();
            if (codeBlock == null)
            {
                Debug.LogError($"Block {block.name} is not a valid CodeBlock.");
                return;
            }

            if(prev != null)
            {
                PayloadResultModel validation = CodeBlock.ValidateBlockPlacement(codeBlock, prev);
                if (validation.Success == false)
                {
                    Debug.LogError(validation.Message);
                    successErrorManagerScreen.SetStatusErrorScreen(true, validation.Message);
                    return;
                }
            }
            prev = codeBlock;
            codeBlocks.Add(codeBlock);
        }

        List<CodeBlock> postFix = ExpressionTreeBuilder.ToPostfix(codeBlocks);
        CodeBlock root = ExpressionTreeBuilder.BuildExpressionTree(postFix);
        if (root == null)
        {
            successErrorManagerScreen.SetStatusErrorScreen(true, "Fill them in a Correct Order!!");
            Debug.LogError("Failed to build expression tree from blocks.");
            return;
        }


        assignmentObject.setLeftChild(variableBlock);
        assignmentObject.setRightChild(root);

        object result = assignmentObject.Evaluate(context);
        // Debug.Log($"Assignment result: {(result is VariableBlock ? "Yes" : "maklo")}");
        if (result is PayloadResultModel payload)
        {
            if (payload.Payload is VariableBlock blockResult)
            {

                if (payload.Success)
                {
                    BlockHolder secondHolder = secondPart.GetComponentInChildren<BlockHolder>();
                    if (secondHolder == null)
                    {
                        Debug.LogError("BlockHolder not found in the second part.");
                        return;
                    }
                    //karena dari assignmentnya udah ngasih block yang baru, jadi kita tinggal ambil aja trus cantolin ke holder
                    secondHolder.AddBlock(blockResult.gameObject);
                    ResetContainerState(holder);
                }
                else
                {
                    Destroy(blockResult.gameObject);
                    successErrorManagerScreen.SetStatusErrorScreen(true, payload.Message);
                }
            }
            else
            {
                successErrorManagerScreen.SetStatusErrorScreen(true, payload.Message);
            }

        }
        
    }

    private void ResetContainerState(BaseBlockContainer container)
    {
        // reset state field kalau semuanya berhasil. ( reset free from containernya doang.)
        // Instantiate VariableAdapter baru
        GameObject adapter = firstPart.GetComponentInChildren<VariableBlockAdapter>().gameObject;
        if (adapter == null)
        {
            Debug.LogError("VariableBlockAdapter not found in the first part.");
            return;
        }

        GameObject newAdapter = blockFactory.CreateBlock(BlockType.Variable_Adapter, DataType.CreateDataType<int>(null), firstPart.transform);
        // assign the same position and parent as the old adapter
        newAdapter.transform.position = adapter.transform.position;
        newAdapter.transform.SetParent(adapter.transform.parent);
        // destroy the old adapter
        // Debug.Log($"Destroying adapter {adapter.name} and replacing it with a new VariableAdapter.");
        Destroy(adapter);
        List<GameObject> blocks = container.GetAllBlocks();

        // Go through each block
        for (int i = 0; i < blocks.Count; i++)
        {
            GameObject block = blocks[i];

            container.RemoveBlock(block);
            Destroy(block);
        }
        container.SetBlocks(blocks);
    }

    private void SetUpSequences(FreeBlockContainer container)
    {
        List<GameObject> codeBlocks = container.GetAllBlocks();
        if (codeBlocks.Count == 0)
        {
            Debug.LogError($"No code blocks found in the {container.gameObject.name} ");
            return;
        }
        foreach (GameObject block in container.GetAllBlocks())
        {
            if (block.GetComponent<CodeBlock>() != null)
            {
                Button b = block.GetComponent<Button>();
                if (b == null)
                {
                    Debug.LogError($"Block {block.name} does not have a Button component.");
                    continue;
                }
                b.onClick.RemoveAllListeners();

            }
            if (block.GetComponent<ParenthesisBlock>() != null)
            {
                block.GetComponentInChildren<TextMeshProUGUI>().text = block.GetComponent<ParenthesisBlock>().ToString();
            }
        }
    }

    public void ExecuteSequence(FreeBlockContainer container)
    {
        successErrorManagerScreen.SetCurrentScreen(secondPart.transform.parent.gameObject.transform.parent.gameObject);
        List<GameObject> blocks = container.GetAllBlocks();
        if (blocks.Count == 0)
        {
            Debug.LogError("ExecuteSequence: No blocks to execute in the sequence.");
            return;
        }

        List<CodeBlock> codeBlocks = new List<CodeBlock>();
        foreach (GameObject block in blocks)
        {
            if (block.GetComponent<BlockSlot>() != null)
            {
                Debug.Log($"Block {block.name} is a BlockSlot, retrieving its block.");
                codeBlocks.Add(block.GetComponent<BlockSlot>().GetBlock());
                continue;
            }

            CodeBlock codeBlock = block.GetComponent<CodeBlock>();
            if (codeBlock == null)
            {
                Debug.LogError($"Block {block.name} is not a valid CodeBlock.");
                return;
            }
            codeBlocks.Add(codeBlock);

        }
        codeBlocks = ExpressionTreeBuilder.ToPostfix(codeBlocks);
        CodeBlock root = ExpressionTreeBuilder.BuildExpressionTree(codeBlocks);
        if (codeBlocks.Count == 0)
        {
            Debug.LogError("ExecuteSequence: No valid code blocks found after conversion to postfix.");
            return;
        }

        object result = root.Evaluate(context);
        if(result is bool boolResult)
        {
            if (boolResult)
            {
                //Debug.Log("HORE BENAR GOBLOK");
                successErrorManagerScreen.SetStatusSuccesScreen(true);
                Solve();
            }
            else
            {
                successErrorManagerScreen.SetStatusErrorScreen(true, "Do It Right.");
                dialogueSystem.StartDialogue(RoomID.Room2, DialogueID.Failed);
            }
                
        }
        else if(result is PayloadResultModel payloadResult)
        {
            successErrorManagerScreen.SetStatusErrorScreen(true, payloadResult.Message);

        }
        else
        {
            successErrorManagerScreen.SetStatusErrorScreen(true, "ExecuteSequence: Result is not a valid CodeBlock.");
            Debug.LogError("ExecuteSequence: Result is not a valid CodeBlock.");
        }



    }

    public void ShowSecondPart()
    {
        firstPart.SetActive(false);
        secondPart.SetActive(true);
    }
}