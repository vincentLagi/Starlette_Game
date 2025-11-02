using System.Collections.Generic;
using UnityEngine;



public static class ArithmeticValidator
{
    public static AritmethicValidationError Validate(List<CodeBlock> codeBlocks)
    {
        AritmethicValidationError validationError = new AritmethicValidationError();

        foreach (var block in codeBlocks)
        {
            if (block is ArithmeticOperatorBlock operatorBlock)
            {
                validationError = ValidateOperatorBlock(operatorBlock);
                if (validationError.ErrorType != AritmethicErrorType.None)
                {
                    return validationError;
                }
            }
        }

        return validationError;
    }

    private static AritmethicValidationError ValidateOperatorBlock(ArithmeticOperatorBlock operatorBlock)
    {
        AritmethicValidationError validationError = new AritmethicValidationError();
        CodeBlock leftOperand = operatorBlock.getLeftChild();
        CodeBlock rightOperand = operatorBlock.getRightChild();

        if (leftOperand == null || rightOperand == null)
        {
            validationError.ErrorType = AritmethicErrorType.ConsecutiveOperand;
            validationError.ErrorMessage = "Operator block has no operands";
            validationError.BlockLocation = operatorBlock;
            return validationError;
        }

        if (leftOperand is OperatorBlock || rightOperand is OperatorBlock)
        {
            validationError.ErrorType = AritmethicErrorType.ConsecutiveOperand;
            validationError.ErrorMessage = "Operator block has consecutive operands";
            validationError.BlockLocation = operatorBlock;
            return validationError;
        }

        if (rightOperand is LiteralBlock literalBlock && Integer.ParseValue(literalBlock.Evaluate()) == 0)
        {
            validationError.ErrorType = AritmethicErrorType.DivisionByZero;
            validationError.ErrorMessage = "Division by zero error";
            validationError.BlockLocation = operatorBlock;
        }
        return validationError;
    }
}
