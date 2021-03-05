namespace SQSConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            SQSOperations operations = new SQSOperations();
            operations.AddTagQueue();
        }
    }
}
