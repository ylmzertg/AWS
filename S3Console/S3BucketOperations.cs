using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace S3Console
{
    public class S3BucketOperations : IDisposable
    {
        #region Variables
        AmazonS3Client client;
        const string bucketName = "myapp1238521478";
        const string keyValue = "test.txt";
        public BasicAWSCredentials credentials = new BasicAWSCredentials
                            (ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"]);
        #endregion

        public S3BucketOperations()
        {
            client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.EUWest1);
        }

        [Obsolete]
        public void CreateBucket()
        {
            if (AmazonS3Util.DoesS3BucketExist(client, "myapp1238521478"))
            {
                Console.WriteLine("Bucket Already exist");
            }
            else
            {
                var bucketRequest = new PutBucketRequest
                {
                    BucketName = "myapp1238521478",
                    UseClientRegion = true
                };
                var bucketResponse = client.PutBucket(bucketRequest);

                if (bucketResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Crated bucket successfully");
                }
            }
        }

        public void UploadFile()
        {
            //var transferUtility = new TransferUtility(client);
            //transferUtility.Upload(AppDomain.CurrentDomain.BaseDirectory + "\\test.txt",bucketName);
            //Console.WriteLine("File Uploaded Successfully");
            //Console.ReadLine();

            //var transferUtility = new TransferUtility(client);
            //transferUtility.UploadDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\example", bucketName);
            //Console.WriteLine("File Uploaded Successfully");
            //Console.ReadLine();

            var transferUtility = new TransferUtility(client);
            var transferForRequest = new TransferUtilityUploadRequest
            {
                FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\test.txt",
                CannedACL = S3CannedACL.PublicRead,
                BucketName = bucketName
            };
            transferUtility.Upload(transferForRequest);
            Console.WriteLine("File Uploaded Successfully");
            Console.ReadLine();
        }

        public async Task DownloadFileAsync()
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = "test.txt"
            };

            using (GetObjectResponse response = await client.GetObjectAsync(request))
            {
                using (Stream responseStream = response.ResponseStream)
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string content = reader.ReadToEnd();
                        var contentype = response.Headers["Content-Type"];

                        Console.WriteLine("File Content: ");
                        Console.WriteLine(content);

                        Console.WriteLine("File Content-Type: ");
                        Console.WriteLine(contentype);
                    }
                }
            }
            Console.WriteLine("File Download");
            Console.ReadLine();
        }

        public void PresSignedUrl()
        {
            try
            {
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = "test.txt",
                    Expires = DateTime.Now.AddMinutes(5),
                    Verb = HttpVerb.GET
                };

                var result = client.GetPreSignedURL(request);
                Console.WriteLine("URL String");
                Console.WriteLine(result);

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error Message Amazon: '{0}'", e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error Message Custom: '{0}'", e.Message);
            }
            Console.ReadLine();
        }

        public void GetObjectTagging()
        {
            GetObjectTaggingRequest request = new GetObjectTaggingRequest
            {
                BucketName = bucketName,
                Key = "test.txt",
            };

            GetObjectTaggingResponse response = client.GetObjectTagging(request);
            if (response.Tagging.Count == 0)
                Console.WriteLine("Tags not Found");

            foreach (var tag in response.Tagging)
            {
                Console.WriteLine($"Key:{tag.Key} , Value : {tag.Value}");
            }
            Console.ReadLine();
        }

        public void UpdateObjectTagging()
        {
            Tagging tags = new Tagging();
            tags.TagSet = new List<Tag>
            {
                new Tag{Key = "TagKey1",Value="TagValue1"},
                new Tag{Key = "TagKey2",Value="TagValue2"},
            };

            PutObjectTaggingRequest request = new PutObjectTaggingRequest 
            {
                BucketName = bucketName,
                Key="test.txt",
                Tagging = tags
            };

            PutObjectTaggingResponse response = client.PutObjectTagging(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("tags updated successfully");
            }
            Console.ReadLine();
            GetObjectTagging();
        }

        public void UpdateObjectACL()
        {
            PutACLRequest request = new PutACLRequest
            {
                BucketName = bucketName,
                Key = keyValue,
                CannedACL = S3CannedACL.PublicRead
            };

            var response = client.PutACL(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                Console.WriteLine("Updated Object ACL");
            Console.ReadLine();
        }

        public void BucketVersion()
        {
            PutBucketVersioningRequest request = new PutBucketVersioningRequest
            {
                BucketName = bucketName,
                VersioningConfig = new S3BucketVersioningConfig
                {
                    EnableMfaDelete = false,
                    Status = VersionStatus.Enabled
                }
            };

            var response = client.PutBucketVersioning(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                Console.WriteLine("Versioning Successfull");
            Console.ReadLine();
        }

        public void BucketAccelerate()
        {
            PutBucketAccelerateConfigurationRequest request = new PutBucketAccelerateConfigurationRequest
            {
                BucketName = bucketName,
                AccelerateConfiguration = new AccelerateConfiguration { Status = BucketAccelerateStatus.Enabled}
            };

            var response = client.PutBucketAccelerateConfiguration(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                Console.WriteLine("Accelerate Success");
            Console.ReadLine();
        }

        public void Dispose()
        {
            client.Dispose();
        }

        #region Unneceserray
        //Another DownloadFileAsync
        //public async Task ReadObjectDataAsync()
        //{
        //    string responseBody = "";
        //    try
        //    {
        //        GetObjectRequest request = new GetObjectRequest
        //        {
        //            BucketName = bucketName,
        //            Key = "test.txt"
        //        };

        //        using (GetObjectResponse response = await client.GetObjectAsync(request))
        //        using (Stream responseStream = response.ResponseStream)
        //        using (StreamReader reader = new StreamReader(responseStream))
        //        {
        //            string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
        //            string contentType = response.Headers["Content-Type"];
        //            Console.WriteLine("Object metadata, Title: {0}", title);
        //            Console.WriteLine("Content type: {0}", contentType);

        //            responseBody = reader.ReadToEnd(); // Now you process the response body.
        //        }
        //    }
        //    catch (AmazonS3Exception e)
        //    {
        //        Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
        //    }
        //} 
        #endregion
    }
}
