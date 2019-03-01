using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Threading.Tasks;
using Amazon.Runtime;

namespace sql2s3
{
   public class S3Service
   {
       private IAmazonS3 client;
       private string bucketName;
       private string bucketPrefix;

       public S3Service(string bucketName, string bucketPrefix = null)
       {
           //use different profile for credentials
            var credentialProfileStoreChain = new Amazon.Runtime.CredentialManagement.CredentialProfileStoreChain();
            if (credentialProfileStoreChain.TryGetAWSCredentials("myprofile", out AWSCredentials credentials))
            {
                var config = new AmazonS3Config();
                config.ServiceURL = "s3.amazonaws.com";
                config.RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName("us-east-1");
                this.client = new AmazonS3Client(credentials, config);
            }
            this.bucketName = bucketName;
            if (!String.IsNullOrEmpty(bucketPrefix))
                this.bucketPrefix = bucketPrefix;
       }

       public async Task GetS3BucketName()
       {
            ListBucketsResponse response = await client.ListBucketsAsync();
            foreach (S3Bucket bucket in response.Buckets)
            {
                Console.WriteLine("You own Bucket with name: {0}", bucket.BucketName);
            }
       }

       public async Task UploadContentToS3(string fileName, string content)
       {
           try
           {
               var request = new PutObjectRequest()
               {
                    BucketName = bucketName,
                    Key = $"{bucketPrefix}/{fileName}",
                    ContentBody = content,
                    TagSet = new List<Tag>{
                        new Tag { Key = "processed", Value = "0"}
                    }
               };

               var response = await client.PutObjectAsync(request);
           }
           catch (AmazonS3Exception amazonS3Exception)
           {
               if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
               {
                   Console.WriteLine("Please check the provided AWS Credentials.");
               }
               else
               {
                   Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
               }
           }
       }

       public async Task GetFile(string bucketName, string filename)
       {
            try
            {
                var response = await client.GetObjectAsync(bucketName, filename);
                using (var reader = new StreamReader(response.ResponseStream))
                {
                    Console.WriteLine(await reader.ReadToEndAsync());
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
       }

       public async Task GetFileList(string bucketName, string prefix = null)
       {
            try
            {
                ListObjectsRequest request = new ListObjectsRequest()
                {
                    BucketName = bucketName,
                    // list only things starting with "foo"
                    Prefix = prefix
                    // only list 3 things
                    //MaxKeys = 3;
                };
                //make the call
                string token = null;
                var result = new List<S3Object>();
                do
                {
                    ListObjectsResponse response = await client.ListObjectsAsync(request).ConfigureAwait(false);
                    result.AddRange(response.S3Objects);
                    token = response.NextMarker;

                } while (token != null);

                //loop thru results
                foreach (S3Object obj in result)
                {
                    Console.WriteLine("key = {0} size = {1}", obj.Key, obj.Size);
                }
            }
           catch (AmazonS3Exception amazonS3Exception)
           {
               if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
               {
                   Console.WriteLine("Please check the provided AWS Credentials.");
                   Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
               }
               else
               {
                   Console.WriteLine("An error occurred with the message '{0}' when listing objects", amazonS3Exception.Message);
               }
           }
       }
   }
}