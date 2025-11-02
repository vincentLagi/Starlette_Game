
using UnityEngine;

public class Boolean : DataType
{


    public static Boolean GetRandomValue()
    {
        Boolean randomBool = new()
        {
            Value = Random.value > 0.5f
        };
        return randomBool;
    }

    public static bool ParseValue(object value)
    {
        return (bool)value;
    }

    public override bool IsValidValue(object value)
    {
        return value is bool;
    }

    public override string ToString()
    {
        return ((bool)Value).ToString();
    }
}
