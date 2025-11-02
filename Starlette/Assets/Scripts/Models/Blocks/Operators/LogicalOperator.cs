
using System;
using TMPro;
using UnityEngine;

public enum LogicalOperatorType
{
    AND,
    OR,
    Random
}
public class LogicalOperator : OperatorBlock
{
    public LogicalOperatorType Operator;

    
    public LogicalOperator(CodeBlock left, CodeBlock right, LogicalOperatorType op)
    {
        leftOperandBlock = left;
        rightOperandBlock = right;
        Operator = op;
    }

    public override object Evaluate(CompilerContext context)
    {
        object leftResult = leftOperandBlock.Evaluate(context);
        object rightResult = rightOperandBlock.Evaluate(context);
        if(leftResult is not bool || rightResult is not bool)
        {
            Debug.LogError(leftResult.GetType() + " " + rightResult.GetType());
            if(leftResult is PayloadResultModel leftError)
            {
                Debug.LogError(leftError.Message);
            }
            else if (rightResult is PayloadResultModel rightError)
            {
                Debug.LogError(rightError.Message);
            }
            return PayloadResultModel.ResultError("Logical operator requires both operands to be boolean values.");
        }


        bool leftValue = (bool)leftResult;
        bool rightValue = (bool)rightResult;
        //Debug.Log($"Evaluating Logical: {leftValue} {Operator} {rightValue}");
        return Operator switch
        {
            LogicalOperatorType.AND => leftValue && rightValue,
            LogicalOperatorType.OR => leftValue || rightValue,
            _ => throw new System.Exception("Invalid logical operator")
        };
    }

    public override string ToString()
    {
        return Operator switch
        {
            LogicalOperatorType.AND => "&&",
            LogicalOperatorType.OR => "||",
            _ => throw new System.Exception("Invalid logical operator")
        };
    }

    /// <summary>
    /// Initializes the logical operator with a specific value.
    /// recieves LogicalOperatorType Value
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentException"></exception>
    public override void Init(object value)
    {
        if (value is LogicalOperatorType logicalOperatorType)
        {
            Operator = logicalOperatorType;
        }
        else if(value is LogicalOperator logicalOperator)
        {
            Operator = logicalOperator.Operator;
        }
        else
        {
            throw new ArgumentException("Invalid value type for LogicalOperator");
        }
    }

    protected override void AdditionalAwake()
    {
        if (Operator == LogicalOperatorType.Random)
        {
            Operator = (LogicalOperatorType)UnityEngine.Random.Range(0, 2);
        }   
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = ToString();
    }

    public override int Precedence => 1;
}