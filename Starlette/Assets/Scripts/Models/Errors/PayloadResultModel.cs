
public class PayloadResultModel
{
    public string Message { get; private set; }
    public bool Success { get; private set; }

    public object Payload { get; private set; }

    public PayloadResultModel(string message, bool success, object payload = null)
    {
        Message = message;
        Success = success;
        Payload = payload;
    }

    public override string ToString()
    {
        return $"Error: {Message}\n";
    }

    public static PayloadResultModel ResultSuccess(string message, object payload = null)
    {
        return new PayloadResultModel(message, true, payload);
    }

    public static PayloadResultModel ResultError(string message, object payload = null)
    {
        return new PayloadResultModel(message, false, payload);
    }
}