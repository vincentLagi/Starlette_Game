using UnityEngine;


public enum AritmethicErrorType
{
    None, DivisionByZero, WrongOperatorPosition, ConsecutiveOperand, WrongOperandType
}
public class AritmethicValidationError
{
    public AritmethicErrorType ErrorType { get; set; }
    public string ErrorMessage { get; set; }
    public CodeBlock BlockLocation { get; set; }

    public AritmethicValidationError()
    {
        ErrorType = AritmethicErrorType.None;
        ErrorMessage = string.Empty;
        BlockLocation = null;
    }
    public AritmethicValidationError(AritmethicErrorType errorType, string errorMessage, CodeBlock blockLocation)
    {
        ErrorType = errorType;
        ErrorMessage = errorMessage;
        BlockLocation = blockLocation;
    }
}