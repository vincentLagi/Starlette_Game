using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    BlockFactory factory;
    [SerializeField] private CompilerContext context;
    public SuccessErrorManagerScreen successErrorManagerScreen;
    public string puzzleID;
    public RoomID roomID;
    private bool isDone = false;
    void Start()
    {
        RoomProgressManager.Instance.RegisterPuzzle(roomID, puzzleID);
        if (context == null)
        {
            context = GetComponentInParent<CompilerContext>();
            if (context == null)
            {
                Debug.LogError("CompilerContext not found in the scene.");
                return;
            }
        }
        factory = GameObject.FindFirstObjectByType<BlockFactory>();
        if (factory == null)
        {
            Debug.LogError("BlockFactory not found in the scene.");
            return;
        }
        BlockHolder holder = GetComponentInChildren<BlockHolder>();
        if (holder == null)
        {
            Debug.LogError("BlockHolder component not found in children.");
            return;
        }

        AssignmentOperatorBlock freeBlockContainer = GetComponentInChildren<AssignmentOperatorBlock>();
        Button operatorButton = freeBlockContainer.gameObject.GetComponent<Button>();
        operatorButton.onClick.RemoveAllListeners();

        for (int i = 0; i < 3; i++)
        {
            RandomizeBlocks(holder);
        }
        
        // make static add block Addition and A Float Block
        // GameObject additionBlock = factory.CreateBlock(BlockType.ArithmeticOperator, ArithmeticType.Add, holder.transform);
        // holder.AddBlock(additionBlock);
        // GameObject multiplyBlock = factory.CreateBlock(BlockType.ArithmeticOperator, ArithmeticType.Multiply, holder.transform);
        // holder.AddBlock(multiplyBlock);
        // GameObject intBlock = factory.CreateBlock(BlockType.Literal_Int, Integer.GetRandomValue(), holder.transform);
        // holder.AddBlock(intBlock);
        // GameObject intBlock2 = factory.CreateBlock(BlockType.Literal_Int, Integer.GetRandomValue(), holder.transform);
        // holder.AddBlock(intBlock2);
    }
    public void Solve()
    {
        isDone = true;
        RoomProgressManager.Instance.MarkPuzzleFinished(roomID, puzzleID);
    }

    public bool GetIsDone()
    {
        return isDone;
    }
    private void RandomizeBlocks(BlockHolder holder)
    {

        BlockType randomType = GetRandomType();
        if (randomType == BlockType.Literal_Float)
        {
            FloatType randomValue = FloatType.GetRandomValue();
            GameObject block = factory.CreateBlock(randomType, randomValue, holder.transform);
            holder.AddBlock(block);
        }
        else if (randomType == BlockType.Literal_Int)
        {
            Integer randomValue = Integer.GetRandomValue();
            GameObject block = factory.CreateBlock(randomType, randomValue, holder.transform);
            holder.AddBlock(block);
        }
        else if (randomType == BlockType.Literal_Bool)
        {
            Boolean randomValue = Boolean.GetRandomValue();
            GameObject block = factory.CreateBlock(randomType, randomValue, holder.transform);
            holder.AddBlock(block);
        }

    }

    private BlockType GetRandomType()
    {
        int randomIndex = Random.Range(0, 3);
        return randomIndex switch
        {
            0 => BlockType.Literal_Float,
            1 => BlockType.Literal_Int,
            2 => BlockType.Literal_Bool,
            _ => BlockType.Literal_Int // Default case
        };
    }

    public void ExecuteSequence(BaseBlockContainer holder)
    {
        successErrorManagerScreen.SetCurrentScreen(this.gameObject.transform.parent.gameObject);
        // Implementation for executing the code block
        if (holder == null)
        {
            // bukain panel nanti
            Debug.LogError("Holder is null, cannot execute sequence.");
            return;
        }
        List<GameObject> blocks = holder.GetAllBlocks();
        GameObject variable = blocks[0];
        VariableBlock variableBlock = variable.GetComponent<VariableBlock>();
        Debug.Log($"{(variableBlock.GetValue().GetValue() is Integer ? "Integer" : "Not Integer")}");


        GameObject assignment = blocks[1];
        PayloadResultModel variableCheck = VariableBlockAdapter.IsValidVariableName(variableBlock.VariableName);
        if (variableCheck.Success == false)
        {
            // open panel
            successErrorManagerScreen.SetStatusErrorScreen(true, $"{variableCheck.Message}");
            Debug.LogError($"{variableCheck.Message}");
            return;
        }

        AssignmentOperatorBlock assignmentObject = assignment.GetComponent<AssignmentOperatorBlock>();
        if (assignmentObject == null)
        {
            Debug.LogError("Assignment block is not of type AssignmentOperatorBlock.");
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


        codeBlocks.RemoveAt(0);
        codeBlocks.RemoveAt(0);
        List<CodeBlock> postFix = ExpressionTreeBuilder.ToPostfix(codeBlocks);
        Debug.Log($"Postfix expression: {string.Join(", ", postFix)}");
        CodeBlock root = ExpressionTreeBuilder.BuildExpressionTree(postFix);
        Debug.Log($"Root of expression tree: {((LiteralBlock)root).GetValue().GetValue()}");
        if (root == null)
        {
            Debug.LogError("Failed to build expression tree from blocks.");
            return;
        }
        assignmentObject.setLeftChild(variableBlock);
        assignmentObject.setRightChild(root);

        object result = assignmentObject.Evaluate(context);
        Debug.Log($"Result of assignment: {result}");
        if (result is VariableBlock a)
        {
            Destroy(a.gameObject);
            ResetContainerState(holder);
          
        }
        else if (result is PayloadResultModel payloadResultModel)
        {
            if (payloadResultModel.Payload is VariableBlock blockResult)
            {
                if (payloadResultModel.Success)
                {
                    Destroy(blockResult.gameObject);
                    ResetContainerState(holder);
                }
                else
                {
                    successErrorManagerScreen.SetStatusErrorScreen(true, payloadResultModel.Message);
                    Destroy(((VariableBlock)payloadResultModel.Payload).gameObject);
                }
            }
            else
            {
                successErrorManagerScreen.SetStatusErrorScreen(true, payloadResultModel.Message);
            }
        }

        if (gameObject.GetComponentInChildren<BlockHolder>().GetAllBlocks().Count == 0)
        {
            successErrorManagerScreen.SetStatusSuccesScreen(true);
            this.gameObject.SetActive(false);
            Solve();
        }

    }



    private void ResetContainerState(BaseBlockContainer container)
    {
        // reset state field kalau semuanya berhasil. ( reset free from containernya doang.)
        // Instantiate VariableAdapter baru
        List<GameObject> blocks = container.GetAllBlocks();

        // Go through each block
        for (int i = 0; i < blocks.Count; i++)
        {
            GameObject block = blocks[i];

            if (i == 0 && (block.GetComponent<VariableBlock>() != null || block.GetComponent<VariableBlockAdapter>() != null))
            {
                // This is the first block: a VariableAdapter or VariableBlock

                // Instantiate a new VariableAdapter and set its parent
                GameObject newAdapter = factory.CreateBlock(BlockType.Variable_Adapter, DataType.CreateDataType<int>(null), container.transform);

                // Destroy the old block
                Debug.Log($"Destroying block {block.name} and replacing it with a new VariableAdapter.");
                Destroy(block);

                // Optional: Update the list if needed
                blocks[i] = newAdapter;
            }
            else if (i > 0 && block.GetComponent<BlockSlot>() != null)
            {
                // This is a BlockSlot after the first index
                Debug.Log($"Clearing block slot {block.name}.");
                block.GetComponent<BlockSlot>().DestroyBlock();
            }
        }
        container.SetBlocks(blocks);
        // Optionally, you can also reset the container's state or UI if needed
    }
}
