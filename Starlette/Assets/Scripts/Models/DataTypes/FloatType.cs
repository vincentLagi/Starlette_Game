using System;
using UnityEngine;


public class FloatType : DataType
{

    public static FloatType GetRandomValue()
    {
        FloatType randomFloat = new()
        {
            Value = UnityEngine.Random.Range(1f, 21f) // Random float between 1 and 21
        };
        return randomFloat;
    }
    
    public static float ParseValue(object value)
    {
        return Convert.ToSingle(value);
    }

    public override bool IsValidValue(object value)
    {
        return value is float;
    }

    public override string ToString()
    {
        return ((float)Value).ToString();
    }
}