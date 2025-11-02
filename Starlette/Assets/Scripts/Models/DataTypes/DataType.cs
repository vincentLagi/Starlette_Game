
using UnityEngine.UIElements.Experimental;

public abstract class DataType : IStringable
{
    public object Value { get; set; }

    public abstract bool IsValidValue(object value);
    public object GetValue() => Value;
    public abstract override string ToString();

    public static DataType CreateDataType<T>(object value = null)
    {
        return typeof(T) switch
        {
            var type when type == typeof(bool) => new Boolean { Value = value == null ? false : value },
            var type when type == typeof(int) => new Integer { Value = value == null ? 0 : value },
            var type when type == typeof(float) => new FloatType { Value = value == null ? 0f : value },
            _ => throw new System.Exception($"Unknown data type: {typeof(T).Name}")
        };
    }
}
