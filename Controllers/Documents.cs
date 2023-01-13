using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using ConstructionApi.Contracts;
using ConstructionApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConstructionApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Documents : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;


        public Documents(IAmazonS3 s3Client) {
            _s3Client = s3Client;
        }


        [HttpPost]
        public async Task<bool> CreateFile(IFormFile file, [FromHeader] int customerId)
        {
            /*using (var client = new TransferUtility(_s3Client))
            {
                var uploudRequest = new TransferUtilityUploadRequest
                {
                    BucketName = "test-p001",
                    Key = "hel",
                    InputStream = file.OpenReadStream(),
                    ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256,
                    ContentType = "pdf"

            };

                client.Upload(uploudRequest);

                return true;


            }*/

            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = "construction001";
            request.InputStream = file.OpenReadStream();
            request.Key = customerId + "/orders/" + file.FileName;
            request.ContentType = file.ContentType;
            var response = await _s3Client.PutObjectAsync(request);
            if (response!=null) { return true; }
            else { return false; }
        }
        

        [HttpGet]
        public async Task<IActionResult> DownloadFile(string fileName, [FromHeader] int customerId)
        {



            /*GetObjectRequest request = new GetObjectRequest();
            request.BucketName = "test-p001";
            request.Key = "0/1651531557-testf.pdf";
            var response = await _s3Client.GetObjectAsync(request);
            var f = response.ResponseStream;
            return Ok(response.ResponseStream);*/
            var name = $"{customerId}/orders/{fileName}";
            var file = await GetFile(name);
            

            return file;
        }

        async Task<FileStreamResult> GetFile(string name) {
            

            var fileTransferUtility = new TransferUtility(_s3Client);

            var objectResponse = await fileTransferUtility.S3Client.GetObjectAsync(new GetObjectRequest()
            {
                BucketName = "construction001",
                Key = name
            });

            if (objectResponse.ResponseStream == null)
            {
                return null;
            }
            string mimeType = objectResponse.Headers.ContentType;
            
            var formFile = new FileStreamResult(objectResponse.ResponseStream, mimeType)
            {
                
            };
            HttpContext.Response.Headers.Add("content-disposition", "inline; filename=helow.pdf");

            return formFile;

        }
        [HttpDelete("delete/{id}")]
        public async Task<bool> DeleteFile(string bucket,string key) {
            var response = await _s3Client.DeleteObjectAsync(bucket,key);
            if (response != null) { return true; }
            else { return false; }
        }

        [HttpPost("/pp")]
        public async Task<IActionResult> StorenS3File(IFormFile file, [FromHeader] int customerId)
        {
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = "construction001";
            request.InputStream = file.OpenReadStream();
            request.Key = customerId + "/orders/" + file.FileName;
            request.ContentType = file.ContentType;
            try { 
            var response = await _s3Client.PutObjectAsync(request);

            return Ok(response);
            }
            catch (Exception ex)
             {
                if (ex != null)
                {
                    return Ok(ex.Message);
                }
                else { 
                    return Ok("Something wrong"); 
                }
            }

        }

    }
}
