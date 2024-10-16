namespace WebCalculator.Core.Interfaces
{
    public interface IOperationPriority
    {
        Priority GetPriority(Operation op);
    }
}
