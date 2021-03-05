using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace DynamoDbProject
{
    public class DynamoDBOperations
    {
        #region Variables
        AmazonDynamoDBClient client;
        public BasicAWSCredentials credentials = new BasicAWSCredentials
                            (ConfigurationManager.AppSettings["accessId"],
                            ConfigurationManager.AppSettings["secretKey"]);
        const string tableName = "DynamoDbEmployee";
        const string tableBackupName = "DynamoDbEmployeeBackup";
        const string tableARN = "arn:aws:dynamodb:eu-west-1:109720996851:table/DynamoDbEmployee";
        const string backupTableARN = "arn:aws:dynamodb:eu-west-1:109720996851:table/DynamoDbEmployee/backup/01581782010105-644c1106";
        #endregion

        #region Constructor
        public DynamoDBOperations()
        {
            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.EUWest1);
        }
        #endregion

        #region CustomMethods

        public void CreateTable()
        {
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName ="Id",
                        AttributeType ="N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName ="EmployeeName",
                        AttributeType ="S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "EmployeeName",
                        KeyType = "RANGE"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 25,
                    WriteCapacityUnits = 10
                }
            };

            var response = client.CreateTable(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Table Create Successfully");
            }
            Console.ReadLine();
        }

        public void InsertItem()
        {
            PutItemRequest request = new PutItemRequest
            {
                TableName = tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Id",new AttributeValue { N ="3"} },
                    { "EmployeeName", new AttributeValue{ S ="Mehmet Karagöz"} }
                }
            };

            var response = client.PutItem(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Itemm Added Successfully");
            }
            Console.ReadLine();
        }

        public void GetDataDynamoDbEmployee()
        {
            GetItemRequest request = new GetItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Id",new AttributeValue{ N= "3"} },
                    { "EmployeeName",new AttributeValue{ S="Mehmet Karagöz"} }
                }
            };

            var response = client.GetItem(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                if (response.Item.Count > 0)
                {
                    foreach (var emp in response.Item)
                    {
                        Console.WriteLine($"Key =>{emp.Key}, Value =>{emp.Value.S} {emp.Value.N}");
                    }
                }
                else
                {
                    Console.WriteLine("Item Not Found");
                }
            }
            Console.ReadLine();
        }

        public void DeleteItem()
        {
            DeleteItemRequest request = new DeleteItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Id",new AttributeValue{ N="3"} },
                    {"EmployeeName",new AttributeValue{ S="Mehmet Karagöz"} }
                }
            };
            var response = client.DeleteItem(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Deleted Succcessfully");
            }
            Console.ReadLine();
        }

        public void GetInformationTable()
        {
            DescribeTableRequest request = new DescribeTableRequest
            {
                TableName = "DynamoDbEmployeeBackup"
            };

            var response = client.DescribeTable(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Table ARN =>{response.Table.TableArn}");
                Console.WriteLine($"Table ARN =>{response.Table.TableId}");
                Console.WriteLine($"Table ARN =>{response.Table.TableName}");
            }
            Console.ReadLine();
        }

        public void DeleteTable()
        {
            DeleteTableRequest request = new DeleteTableRequest
            {
                TableName = tableName
            };

            var response = client.DeleteTable(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Table has been Deleted");
                Console.WriteLine($"Table Status=> {response.TableDescription.TableStatus.Value}");
                Console.WriteLine($"Table Name =>{response.TableDescription.TableName}");
            }
            Console.ReadLine();
        }

        public void BackupTable()
        {
            CreateBackupRequest request = new CreateBackupRequest
            {
                TableName = tableName,
                BackupName = tableBackupName
            };
            var response = client.CreateBackup(request);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Backup Created Successfully");
                Console.WriteLine($"Backup ARN =>{response.BackupDetails.BackupArn}");
                Console.WriteLine($"Backup CreateDate =>{response.BackupDetails.BackupCreationDateTime}");
                Console.WriteLine($"Backup Status =>{response.BackupDetails.BackupStatus}");
                Console.WriteLine($"Backup Size Type =>{response.BackupDetails.BackupSizeBytes}");
            }
            Console.ReadLine();
        }

        public void RestoreBackup()
        {
            RestoreTableFromBackupRequest request = new RestoreTableFromBackupRequest 
            {
                BackupArn =backupTableARN,
                TargetTableName= tableBackupName
            };

            var response = client.RestoreTableFromBackup(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Restore Successfully");
                Console.WriteLine($"Table ARN =>{response.TableDescription.TableArn}");
                Console.WriteLine($"Table Status=> {response.TableDescription.TableStatus}");
            }
            Console.ReadLine();
        }
        #endregion
    }
}
