using Amazon.S3;
using Amazon.S3.Model;
using S3AndNet.Interfaces;
using System.Net;

namespace S3AndNet.Services;

public class S3Service : IS3Service
{
    private readonly string _bucketName;
    private readonly IAmazonS3 _s3Client;
    public S3Service(IConfiguration config)
    {
        // Use AWS SDK's default credential chain to pick up credentials from ~/.aws/credentials
        _s3Client = new AmazonS3Client();  // Automatically picks credentials from ~/.aws/credentials
        _bucketName = config["AWS:BucketName"];  // The retrieve bucket name appsettings.json
    }
    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var prefix = "images/";
        var key = prefix + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        using var stream = file.OpenReadStream();

        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = stream,
            ContentType = file.ContentType
        };

        var response = await _s3Client.PutObjectAsync(putRequest);

        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            return key;  // Return the key to access the file.
        }

        throw new Exception("File upload failed.");
    }
}
