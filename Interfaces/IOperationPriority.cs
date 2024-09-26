namespace Calculator
{
    public interface IOperationPriority
    {
        Priority GetPriority(Operation op);
    }
}
