using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class CodeBlock : MonoBehaviour, IStringable
{

    void Awake()
    {
        SetListener();
        AdditionalAwake();
    }

    protected abstract void AdditionalAwake();

    public void SetListener()
    {
        if (gameObject.GetComponent<Button>() == null)
        {
            Button button = gameObject.AddComponent<Button>();
            button.onClick.AddListener(BlockClicked);
        }
        else
        {
            gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            gameObject.GetComponent<Button>().onClick.AddListener(BlockClicked);
        }
    }
    public override abstract string ToString();
    public abstract object Evaluate(CompilerContext context = null);

    public abstract void Init(object value);

    private void BlockClicked()
    {
        // take holder parent
        // transfer this block only 

        BaseBlockContainer container = gameObject.GetComponentInParent<BaseBlockContainer>();
        Debug.Log(container.gameObject.name);
        container.TransferBlockToPartner(gameObject);
    }

    public static PayloadResultModel ValidateBlockPlacement(CodeBlock curr, CodeBlock prev)
    {
        if (curr == null || prev == null)
        {
            return new PayloadResultModel("Invalid block placement.", false);
        }

        // Example validation logic
        if (curr is VariableBlock)
        {
            if (prev is VariableBlock)
            {
                return PayloadResultModel.ResultError("Cannot place two variable blocks next to each other.");
            }
            else if (prev is LiteralBlock)
            {
                return PayloadResultModel.ResultSuccess("Valid placement for variable block.");
            }
        }
        else if (curr is LiteralBlock)
        {
            if (prev is VariableBlock)
            {
                return PayloadResultModel.ResultSuccess("Valid placement for literal block after variable block.");
            }
            else if (prev is LiteralBlock)
            {
                return PayloadResultModel.ResultError("Cannot place two literal blocks next to each other.");
            }
        }
        else if (curr is OperatorBlock)
        {
            if (prev is VariableBlock || prev is LiteralBlock)
            {
                return PayloadResultModel.ResultSuccess("Valid placement for operator block after variable or literal block.");
            }
        }
        else if (curr is LiteralBlock)
        {
            return PayloadResultModel.ResultError("Cannot place two literal blocks next to each other.");
        }
        else if (curr is OperatorBlock && prev is OperatorBlock)
        {
            return PayloadResultModel.ResultError("Cannot place two operator blocks next to each other.");
        }

        return PayloadResultModel.ResultSuccess("Valid block placement.");
    }
}
