namespace Calculator.Interfaces
{
    public interface IMathOperation
    {
        decimal PerformOperation(decimal firstValue, decimal secondValue, Operation op);
    }

}
