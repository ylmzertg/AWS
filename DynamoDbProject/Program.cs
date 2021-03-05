namespace DynamoDbProject
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamoDBOperations operations = new DynamoDBOperations();
            operations.RestoreBackup();
        }
    }
}
