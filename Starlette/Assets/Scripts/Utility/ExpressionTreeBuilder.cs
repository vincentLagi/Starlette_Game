using System.Collections.Generic;
using System.Diagnostics;

public static class ExpressionTreeBuilder
{
    public static CodeBlock BuildExpressionTree(List<CodeBlock> postfix)
    {
        Stack<CodeBlock> stack = new Stack<CodeBlock>();

        foreach (var block in postfix)
        {
            if (block is LiteralBlock || block is VariableBlock)
            {
                stack.Push(block);
            }
            else if (block is OperatorBlock op)
            {
                try
                {
                    op.setRightChild(stack.Pop());
                    op.setLeftChild(stack.Pop());
                }
                catch (System.InvalidOperationException)
                {
                    Debug.WriteLine("Invalid expression: not enough operands for operator.");
                    return null;
                }

                stack.Push(op);
            }
        }

        if (stack.Count == 0)
        {
            Debug.WriteLine("cel");
            return null;
        }
        return stack.Pop(); 
    }

    public static List<CodeBlock> ToPostfix(List<CodeBlock> infix)
    {
        Stack<CodeBlock> operatorStack = new Stack<CodeBlock>();
        List<CodeBlock> output = new List<CodeBlock>();

        foreach (CodeBlock codeBlock in infix)
        {
            if (codeBlock is LiteralBlock || codeBlock is VariableBlock)
            {
                output.Add(codeBlock);
            }
            else if (codeBlock is OperatorBlock op)
            {
                while (operatorStack.Count > 0 &&
                       operatorStack.Peek() is OperatorBlock topOp &&
                       topOp.Precedence >= op.Precedence)
                {
                    output.Add(operatorStack.Pop());
                }
                operatorStack.Push(op);
            }
            else if (codeBlock is ParenthesisBlock p && p.Type == ParenthesisType.Open)
            {
                operatorStack.Push(p);
            }
            else if (codeBlock is ParenthesisBlock pc && pc.Type == ParenthesisType.Close)
            {
                while (operatorStack.Count > 0 && !(operatorStack.Peek() is ParenthesisBlock openParenthesis && openParenthesis.Type == ParenthesisType.Open))
                {
                    output.Add(operatorStack.Pop());
                }
                if (operatorStack.Count > 0)
                {
                    operatorStack.Pop();
                }
            }
        }

        while (operatorStack.Count > 0)
        {
            output.Add(operatorStack.Pop());
        }

        return output;
    }
}