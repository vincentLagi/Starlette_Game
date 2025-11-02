using UnityEngine;

public interface IValue
{
    // later for resolving the value for each operand or operator.
    object Evaluate(CompilerContext context = null);

}