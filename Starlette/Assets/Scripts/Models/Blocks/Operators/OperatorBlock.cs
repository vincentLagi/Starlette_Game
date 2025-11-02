

public abstract class OperatorBlock : CodeBlock
{
    public abstract int Precedence { get; }
    protected CodeBlock rightOperandBlock;
    protected CodeBlock leftOperandBlock;

    public void setLeftChild(CodeBlock left)
    {
        leftOperandBlock = left;
    }

    public void setRightChild(CodeBlock right)
    {
        rightOperandBlock = right;
    }

    public CodeBlock getLeftChild()
    {
        return leftOperandBlock;
    }

    public CodeBlock getRightChild()
    {
        return rightOperandBlock;
    }

    
}
