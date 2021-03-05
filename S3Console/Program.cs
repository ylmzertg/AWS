using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System;
using System.Configuration;

namespace S3Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //S3BucketOperations bucketOperations = new S3BucketOperations();
            //bucketOperations.DownloadFileAsync().Wait();
            //bucketOperations.BucketAccelerate();
            S3GlacierOperations glacierOperations = new S3GlacierOperations();
            glacierOperations.DownloadFile();
        }
    }
}
