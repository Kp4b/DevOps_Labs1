namespace WebCalculator.Core.Interfaces
{
    public interface IMathOperation
    {
        decimal PerformOperation(decimal firstValue, decimal secondValue, Operation op);
    }
}
