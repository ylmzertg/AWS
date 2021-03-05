using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Configuration;

namespace SQSConsoleProject
{
    public class SQSOperations
    {
        #region Variables
        const string queueName = "myAppQueue";
        const string queueURL = "https://sqs.eu-west-1.amazonaws.com/109720996851/myAppQueue";

        public BasicAWSCredentials credentials = new BasicAWSCredentials
                            (ConfigurationManager.AppSettings["accessId"],
                            ConfigurationManager.AppSettings["secretKey"]);

        AmazonSQSClient client;
        #endregion

        #region Constructor
        public SQSOperations()
        {
            client = new AmazonSQSClient(credentials, Amazon.RegionEndpoint.EUWest1);
        }
        #endregion

        #region Methods

        public void CreateSQSQueue()
        {
            CreateQueueRequest request = new CreateQueueRequest
            {
                //QueueName = queueName
                QueueName = "myAppQueue2"
            };

            var response = client.CreateQueue(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("SQS Create Successfully");
                Console.WriteLine($"SQS URL:{response.QueueUrl}");
            }
            Console.ReadLine();
        }

        public void SendMessageWithSQS()
        {
            SendMessageRequest request = new SendMessageRequest
            {
                QueueUrl = queueURL,
                MessageBody = "First SQS Message Body"
            };
            var response = client.SendMessage(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Message Sent Succussfully");
                Console.WriteLine($"Message ID =>{response.MessageId}");
            }
            Console.ReadLine();
        }

        public void ReceiveMessageSQS()
        {
            ReceiveMessageRequest request = new ReceiveMessageRequest
            {
                QueueUrl = queueURL
            };
            var response = client.ReceiveMessage(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Receive Message Successfully");
                foreach (var item in response.Messages)
                {
                    DeleteMessage(item.ReceiptHandle);
                    Console.WriteLine($"Message Content:{item.Body}");
                    Console.WriteLine($"Message ID:{item.MessageId}");
                    Console.WriteLine($"ReceiptHandle:{item.ReceiptHandle}");
                }
            }
            Console.ReadLine();
        }

        public void BatchSendMessages()
        {
            SendMessageBatchRequest batchRequest = new SendMessageBatchRequest();
            batchRequest.QueueUrl = queueURL;
            batchRequest.Entries = new System.Collections.Generic.List<SendMessageBatchRequestEntry>
            {
                new SendMessageBatchRequestEntry {Id = "123456",MessageBody ="First Batch Message Content"},
                new SendMessageBatchRequestEntry {Id = "1234567",MessageBody ="Second Batch Message Content"},
                new SendMessageBatchRequestEntry {Id = "12345678",MessageBody ="Thirt Batch Message Content"}
            };
            var response = client.SendMessageBatch(batchRequest);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Messages queued successfully");
                foreach (var item in response.Successful)
                {
                    Console.WriteLine($"Id=>{item.Id } , MessageID=> {item.MessageId}");
                }
            }
            Console.ReadLine();
        }

        public void DeleteMessage(string receiptHandle)
        {
            DeleteMessageRequest request = new DeleteMessageRequest
            {
                QueueUrl = queueURL,
                ReceiptHandle = receiptHandle
            };

            var response = client.DeleteMessage(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                Console.WriteLine("Message Removed");
            Console.ReadLine();
        }

        public void AllDeleteMessagesByQueueUrl()
        {
            PurgeQueueRequest request = new PurgeQueueRequest
            {
                QueueUrl = queueURL
            };

            var response = client.PurgeQueue(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                Console.WriteLine("All Messages Removed");
            Console.ReadLine();
        }

        public void ListQueues()
        {
            ListQueuesRequest request = new ListQueuesRequest();

            var response = client.ListQueues(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                foreach (var item in response.QueueUrls)
                {
                    Console.WriteLine($"{item}");
                }
            }
            //Console.ReadLine();
        }

        public void DeleteQueue()
        {
            DeleteQueueRequest request = new DeleteQueueRequest { QueueUrl = "https://sqs.eu-west-1.amazonaws.com/109720996851/myAppQueue2" };
            var response = client.DeleteQueue(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Queue has been removed");
                ListQueues();
            }
            Console.ReadLine();
        }

        public void AddTagQueue()
        {
            TagQueueRequest request = new TagQueueRequest();
            request.QueueUrl = queueURL;
            request.Tags.Add("firstTagKey2", "firstTagValue2");
            var response = client.TagQueue(request);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Tag Added Queue Successfully");
                ListTagsQueue();
            }
            Console.ReadLine();
        }

        public void ListTagsQueue()
        {
            ListQueueTagsRequest request = new ListQueueTagsRequest
            {
                QueueUrl = queueURL
            };

            var response = client.ListQueueTags(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                foreach (var item in response.Tags)
                {
                    Console.WriteLine($"tag Key=> {item.Key} , tag Value => { item.Value}");
                }
            }
            //Console.ReadLine();
        }
        #endregion
    }
}
