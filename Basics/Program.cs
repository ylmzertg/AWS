using Amazon.Runtime;
using Amazon.S3;
using System;
using System.Configuration;

namespace Basics
{
    class Program
    {
        static void Main(string[] args)
        {
            //Aws Hesabımıza baglanmamız gerekiyor. Connect to AWS Account
            var credentials = new BasicAWSCredentials
                            (ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"]);

            using (AmazonS3Client client = new AmazonS3Client(credentials,Amazon.RegionEndpoint.EUWest1))
            {
                foreach (var bucket in client.ListBuckets().Buckets)
                {
                    Console.WriteLine(bucket.BucketName + " " + bucket.CreationDate.ToShortDateString());
                }
            }
            Console.ReadLine();
        }
    }
}
