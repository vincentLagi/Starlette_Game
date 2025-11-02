
public enum InitialValueType
{
    None,
    Integer,
    Float,
    Boolean,
}
public class LiteralBlock : CodeBlock
{
    private DataType Value;
    public InitialValueType InitialValueType;

    protected override void AdditionalAwake()
    {
        if (InitialValueType != InitialValueType.None)
        {
            switch (InitialValueType)
            {
                case InitialValueType.Integer:
                    Value = Integer.GetRandomValue();
                    break;
                case InitialValueType.Float:
                    Value = FloatType.GetRandomValue();
                    break;
                case InitialValueType.Boolean:
                    Value = Boolean.GetRandomValue();
                    break;
            }
            gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Value.ToString();
        }
    }

    public void SetValue(DataType value)
    {
        Value = value;
    }

    public DataType GetValue()
    {
        return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override void Init(object value)
    {
        if (value is DataType dataType)
        {
            Value = dataType;
        }
        else if (value is LiteralBlock literalBlock)
        {
            Value = literalBlock.Value;
        }
        else
        {
            throw new System.Exception("Invalid value type for LiteralBlock");
        }
    }

    public override object Evaluate(CompilerContext context = null) => Value.GetValue();
}