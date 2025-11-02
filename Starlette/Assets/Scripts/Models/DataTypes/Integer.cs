
using UnityEngine;

public class Integer : DataType
{
    public static Integer GetRandomValue()
    {
        return new Integer { Value = Random.Range(1, 21) };
    }

    public static int ParseValue(object value)
    {
        return (int)value;
    }

    public override bool IsValidValue(object value)
    {
        return value is int;
    }

    public override string ToString()
    {
        return ((int)Value).ToString();
    }
}