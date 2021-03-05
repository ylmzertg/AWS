using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Configuration;

namespace SNSConsoleProject
{
    public class SNSOperation
    {
        #region Variables
        const string topicName = "myAppTopicName";
        const string topicARN = "arn:aws:sns:eu-west-1:109720996851:myAppTopicName";
        AmazonSimpleNotificationServiceClient client;
        public BasicAWSCredentials credentials = new BasicAWSCredentials
                            (ConfigurationManager.AppSettings["accessId"],
                            ConfigurationManager.AppSettings["secretKey"]);
        #endregion

        #region Constructor
        public SNSOperation()
        {
            client = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.EUWest1);
        }
        #endregion

        #region Methods

        public void CreateTopic()
        {
            CreateTopicRequest request = new CreateTopicRequest
            {
                Name =topicName                
            };

            var response = client.CreateTopic(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Topic Cretead Successfully");
                Console.WriteLine($"Topic ARN:{response.TopicArn}");
            }
            Console.ReadLine();
        }

        public void SubscribeToTopic()
        {
            SubscribeRequest request = new SubscribeRequest 
            {
                TopicArn= "arn:aws:sns:eu-west-1:109720996851:myAppTopicName",
                Protocol = "email",
                Endpoint ="info@noktaatisi.com"
            };
            var response = client.Subscribe(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Subscribe Successfully");
                Console.WriteLine(response.SubscriptionArn);
                Console.WriteLine(request.ReturnSubscriptionArn);
            }
            Console.ReadLine();
        }

        public void PublishTopic()
        {
            PublishRequest request = new PublishRequest 
            {
                TopicArn = topicARN,
                Message="New Message Content",
                Subject ="New Message Topic Subject"
            };

            var response = client.Publish(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Message Send Successfully");
                Console.WriteLine($"Message Id=> {response.MessageId}");
            }
            Console.ReadLine();
        }

        public void ListTopics()
        {
            ListTopicsRequest request = new ListTopicsRequest();
            var response = client.ListTopics(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                foreach (var item in response.Topics)
                {
                    Console.WriteLine(item.TopicArn);
                }
            }
            //Console.ReadLine();
        }

        public void ListSubscriptions()
        {
            ListSubscriptionsByTopicRequest request = new ListSubscriptionsByTopicRequest { TopicArn = topicARN };

            var response = client.ListSubscriptionsByTopic(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                foreach (var sub in response.Subscriptions)
                {
                    Console.WriteLine(sub.Owner);
                    Console.WriteLine(sub.Protocol);
                    Console.WriteLine(sub.SubscriptionArn);
                    Console.WriteLine(sub.Endpoint);
                }
            }
            Console.ReadLine();
        }

        public void Unsubscribe()
        {
            var subscription = "arn:aws:sns:eu-west-1:109720996851:myAppTopicName:f6191ad1-5061-4dec-a84f-fe70ec618cd4";
            UnsubscribeRequest request = new UnsubscribeRequest { SubscriptionArn = subscription };
            var response = client.Unsubscribe(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Unsubscribe Successfully");
            }
            Console.ReadLine();
        }

        public void DeleteTopic()
        {
            Console.WriteLine("List Topic method begin");
            ListTopics();
            Console.WriteLine("List Topic method end");

            DeleteTopicRequest request = new DeleteTopicRequest { TopicArn = topicARN };

            var response = client.DeleteTopic(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Topic has been deleted");
                ListTopics();
            }
            Console.ReadLine();
        }
        #endregion
    }
}
