using Amazon.Glacier;
using Amazon.Glacier.Model;
using Amazon.Glacier.Transfer;
using Amazon.Runtime;
using System;
using System.Configuration;
using System.IO;

namespace S3Console
{
    public class S3GlacierOperations
    {
        #region Variables
        AmazonGlacierClient client;
        const string vaultName = "myapp1238521478";
        public BasicAWSCredentials credentials = new BasicAWSCredentials
                            (ConfigurationManager.AppSettings["accessId"],
                            ConfigurationManager.AppSettings["secretKey"]);
        #endregion

        #region Constructor
        public S3GlacierOperations()
        {
            client = new AmazonGlacierClient(credentials, Amazon.RegionEndpoint.EUWest1);
        }
        #endregion

        #region Methods
        public void CreateVault()
        {
            CreateVaultRequest request = new CreateVaultRequest
            {
                VaultName = vaultName,
                AccountId = "-"
            };

            var response = client.CreateVault(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.Created)
            {
                Console.WriteLine("Create Vault Sucessfully");
            }
            Console.ReadLine();
        }

        public void UploadVaultObject()
        {
            try
            {
                var stream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "\\example");
                UploadArchiveRequest request = new UploadArchiveRequest
                {
                    VaultName = vaultName,
                    AccountId = "",
                    ArchiveDescription = "Test Description Upload",
                    Body = stream,
                    Checksum = TreeHashGenerator.CalculateTreeHash(stream)
                };

                request.StreamTransferProgress += OnUploadProgress;
                var response = client.UploadArchive(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.Created)
                {
                    Console.WriteLine("Archive Upload Successsfully");
                    Console.WriteLine($"RequstId: {response.ResponseMetadata.RequestId}");
                    Console.WriteLine($"ArchiveID {response.ArchiveId}");
                    foreach (var item in response.ResponseMetadata.Metadata)
                    {
                        Console.WriteLine($"{item.Key} / {item.Value}");
                    }
                        
                }
            }
            catch (AmazonGlacierException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private void OnUploadProgress(object sender, StreamTransferProgressArgs e)
        {
            Console.WriteLine($"Percent Done: {e.PercentDone}");
            Console.WriteLine($"Total Transfer: {e.TransferredBytes} / {e.TotalBytes}");
            Console.WriteLine($"Increment Transferred: {e.IncrementTransferred}");
        }

        //static string archiveToUpload = AppDomain.CurrentDomain.BaseDirectory+"\\test.txt";
        //public void UploadVaultObject2()
        //{
        //    try
        //    {
        //        var manager = new ArchiveTransferManager(Amazon.RegionEndpoint.EUWest1);
        //        // Upload an archive.
        //        string archiveId = manager.Upload(vaultName, "getting started archive test", archiveToUpload).ArchiveId;
        //        Console.WriteLine("Copy and save the following Archive ID for the next step.");
        //        Console.WriteLine("Archive ID: {0}", archiveId);
        //        Console.WriteLine("To continue, press Enter");
        //        Console.ReadKey();
        //    }
        //    catch (AmazonGlacierException e) { Console.WriteLine(e.Message); }
        //    catch (AmazonServiceException e) { Console.WriteLine(e.Message); }
        //    catch (Exception e) { Console.WriteLine(e.Message); }
        //    Console.WriteLine("To continue, press Enter");
        //    Console.ReadKey();
        //}

        public void DownloadFile()
        {
            var manager = new ArchiveTransferManager(credentials, Amazon.RegionEndpoint.EUWest1);
            manager.Download(vaultName, "B_CIJkgZDfSRnIgLgsAJBTTRqyWqjMIx_A6n1taETlPWFJLxn4fqBfNc0QDUsfBt18xihbcRJmY8JOfG6Pdr7FK3NFjpe9KbS0h4jM09H89mkMWbDqyH0GN0ZEO-EN1zS4qQdRJYIw", AppDomain.CurrentDomain.BaseDirectory + "\\test-downloadGlacier.txt");
        }
        #endregion

    }
}
