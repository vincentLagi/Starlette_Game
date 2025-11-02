using System;
using TMPro;
using UnityEngine;


public enum ArithmeticType { Add, Substract, Multiply, Divide, Modulo, Random, RandomNoModulo }


public class ArithmeticOperatorBlock : OperatorBlock
{

    public ArithmeticType BlockType;
    protected override void AdditionalAwake()
    {
        if (BlockType == ArithmeticType.RandomNoModulo)
        {
            BlockType = (ArithmeticType)UnityEngine.Random.Range(0, 4);
            // Debug.Log($"Random Arithmetic Operator: {BlockType}, {ToString()}");
        }   
        else if (BlockType == ArithmeticType.Random)
        {
            BlockType = (ArithmeticType)UnityEngine.Random.Range(0, 5);
        }
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = ToString();
    }
    public override object Evaluate(CompilerContext context = null)
    {
        // Anggap left sama right itu udah pasti antara Literal Atau Variable
        // Kalau variable ntar Evaluatenya otomatis ke Evaluatenya literal block
        object left = leftOperandBlock.Evaluate();
        object right = rightOperandBlock.Evaluate();

        Debug.Log($"Evaluating Arithmetic: {left} {BlockType} {right}");

        bool isFloat = left is float || right is float;
        bool isDivide = BlockType == ArithmeticType.Divide;
        bool isModulo = BlockType == ArithmeticType.Modulo;

        int leftInt = 0;
        int rightInt = 0;
        float leftFloat = 0f;
        float rightFloat = 0f;

        // Step 1: Try parse int values
        try
        {
            if (!isFloat)
            {
                leftInt = Integer.ParseValue(left);
                rightInt = Integer.ParseValue(right);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[Int Parse Error] Cannot parse int from: {left} or {right} — {e.Message}");
            return PayloadResultModel.ResultError($"Invalid integer operand: {e.Message}");
        }

        // Step 2: Try parse float values if needed
        try
        {
            if (isFloat || isDivide || isModulo)
            {
                leftFloat = FloatType.ParseValue(left);
                rightFloat = FloatType.ParseValue(right);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[Float Parse Error] Cannot parse float from: {left} or {right} — {e.Message}");
            return PayloadResultModel.ResultError($"Invalid float operand: {e.Message}");
        }

        // Step 3: Division/Modulo by Zero Validation
        try
        {
            if ((isDivide || isModulo))
            {
                if (isFloat && rightFloat == 0f)
                    throw new DivideByZeroException();

                if (!isFloat && rightInt == 0)
                    throw new DivideByZeroException();
            }
        }
        catch (DivideByZeroException e)
        {
            Debug.LogError($"[Zero Division] Divide by zero error: {left} {BlockType} {right}");
            return PayloadResultModel.ResultError("Divide by zero is not allowed.");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Validation Error] While checking divide/modulo by zero: {e.Message}");
            return PayloadResultModel.ResultError(e.Message);
        }

        // Step 4: Check for forced float result due to remainder
        try
        {
            if (!isFloat && isDivide && rightInt != 0 && leftInt % rightInt != 0)
            {
                isFloat = true;
                leftFloat = FloatType.ParseValue(left);
                rightFloat = FloatType.ParseValue(right);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[Modulo Check Error] Cannot determine int remainder: {e.Message}");
            return PayloadResultModel.ResultError($"Error during modulo check: {e.Message}");
        }

        // Step 5: Final Evaluation
        try
        {
            if (isFloat)
            {
                return BlockType switch
                {
                    ArithmeticType.Add => leftFloat + rightFloat,
                    ArithmeticType.Substract => leftFloat - rightFloat,
                    ArithmeticType.Multiply => leftFloat * rightFloat,
                    ArithmeticType.Divide => leftFloat / rightFloat,
                    ArithmeticType.Modulo => leftFloat % rightFloat,
                    _ => throw new NotImplementedException()
                };
            }
            else
            {
                return BlockType switch
                {
                    ArithmeticType.Add => leftInt + rightInt,
                    ArithmeticType.Substract => leftInt - rightInt,
                    ArithmeticType.Multiply => leftInt * rightInt,
                    ArithmeticType.Divide => leftInt / rightInt,
                    ArithmeticType.Modulo => leftInt % rightInt,
                    _ => throw new NotImplementedException()
                };
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[Evaluation Error] Failed evaluating {left} {BlockType} {right}: {e.Message}");
            return PayloadResultModel.ResultError($"Final evaluation error: {e.Message}");
        }
    }

    public override string ToString()
    {
        return BlockType switch
        {
            ArithmeticType.Add => "+",
            ArithmeticType.Substract => "-",
            ArithmeticType.Multiply => "*",
            ArithmeticType.Divide => "/",
            ArithmeticType.Modulo => "%",
            _ => throw new NotImplementedException(),
        };
    }

    public override int Precedence => BlockType switch
    {
        ArithmeticType.Add => 2,
        ArithmeticType.Substract => 2,
        ArithmeticType.Multiply => 3,
        ArithmeticType.Divide => 3,
        ArithmeticType.Modulo => 3,
        _ => 0,
    };

    /// <summary>
    ///  Initializes the arithmetic operator with a specific value.
    /// Receives ArithmeticType value.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentException"></exception>
    public override void Init(object value)
    {
        if (value is ArithmeticType arithmeticType)
        {
            BlockType = arithmeticType;
        }
        else if (value is ArithmeticOperatorBlock arithmeticOperatorBlock)
        {
            BlockType = arithmeticOperatorBlock.BlockType;
        }
        else
        {
            throw new ArgumentException("Invalid value type for ArithmeticOperatorBlock");
        }
    }
}
