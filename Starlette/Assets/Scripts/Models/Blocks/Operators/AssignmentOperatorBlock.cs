using System;
using TMPro;
using UnityEngine;

public enum AssignmentType {Assign, AddAsign}
public class AssignmentOperatorBlock : OperatorBlock
{
    protected override void AdditionalAwake()
    {
        if (BlockType == AssignmentType.Assign)
        {
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = ToString();
        }
        else if (BlockType == AssignmentType.AddAsign)
        {
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "+=";
        }
    }
    public AssignmentType BlockType;
    public void SetOperands(CodeBlock leftOperand, CodeBlock rightOperand)
    {
        leftOperandBlock = leftOperand;
        rightOperandBlock = rightOperand;
    }

    public override object Evaluate(CompilerContext context)
    {
        //left operator will always be a VariableBlock
        if (leftOperandBlock is VariableBlock variableBlock)
        {
            var variableName = variableBlock.VariableName;

            // right operand will always be a CodeBlock
            object value = rightOperandBlock.Evaluate(context);
            //Debug.Log($"Evaluating Assignment: {variableName} to {value}");
            PayloadResultModel result = MatchDataType(variableBlock, value);
            if (!result.Success)
            {
                
                return new PayloadResultModel(result.Message, false);
            }

            // Buat block baru dlu untuk copy hasil variablenya, nanti yang di store ke context itu adalah blck barunya
            BlockFactory blockFactory = BlockFactory.Instance;
            GameObject newVariableBlock  = blockFactory.CreateBlock(BlockFactory.GetBlockTypeFromVariables(variableBlock),
            variableBlock, gameObject.transform.parent);

            LiteralBlock newValue = newVariableBlock.AddComponent<LiteralBlock>();
            newValue.Init(result.Payload);
            VariableBlock copiedVariable = newVariableBlock.GetComponent<VariableBlock>();
            copiedVariable.VariableName = variableName;
            copiedVariable.SetValue(newValue);

            PayloadResultModel declareResult = context.DeclareVariable(variableName);
            if (!declareResult.Success)
            {
                Debug.LogError($"{declareResult.Message}");
                return new PayloadResultModel(declareResult.Message, false, newVariableBlock.GetComponent<VariableBlock>());
            }
            declareResult = context.AssignVariable(variableName, newVariableBlock.GetComponent<VariableBlock>());
            if (!declareResult.Success)
            {
                Debug.LogError($"{declareResult.Message}");
                return new PayloadResultModel(declareResult.Message, false, newVariableBlock.GetComponent<VariableBlock>());
            }

            return PayloadResultModel.ResultSuccess($"Assignment successful: {variableName}, newVariableValue: {newValue.GetValue().GetValue()}", newVariableBlock.GetComponent<VariableBlock>());
        }
        else
        {
            throw new Exception("Left operand must be a VariableBlock");
        }
    }

    private PayloadResultModel MatchDataType(VariableBlock variableBlock, object result)
    {
        DataType variableType = variableBlock.GetDataType();
        if (variableType is Integer && result is int)
        {
            variableType.Value = Integer.ParseValue(result);
        }
        else if (variableType is FloatType && result is float)
        {
            variableType.Value = FloatType.ParseValue(result);
        }
        else if (variableType is Boolean && result is bool)
        {
            variableType.Value = Boolean.ParseValue(result);
        }
        else if (result is null)
        {
            return new PayloadResultModel("Assignment result is null.", false);
        }
        else
        {
            return new PayloadResultModel("Data Type Mismatch.", false);
        }

        return new PayloadResultModel("Execution Successful.", true, variableType);
    }

    public override string ToString()
    {
        return "=";
    }
    public override void Init(object value)
    {
        if (value is AssignmentType assignmentType)
        {
            BlockType = assignmentType;
        }
        else if (value is AssignmentOperatorBlock assignmentOperatorBlock)
        {
            BlockType = assignmentOperatorBlock.BlockType;
        }
        else
        {
            throw new ArgumentException("Invalid value type for AssignmentOperatorBlock");
        }
    }
    public override int Precedence => BlockType switch
    {
        AssignmentType.Assign => 0,
        AssignmentType.AddAsign => 0,
        _ => throw new NotImplementedException(),
    };
}   