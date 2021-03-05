namespace SNSConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            SNSOperation operation = new SNSOperation();
            operation.DeleteTopic();
        }
    }
}
