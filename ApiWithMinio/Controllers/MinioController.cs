using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;

namespace MyMinioApp.Controllers;


[ApiController]
[Route("[controller]")]
[IgnoreAntiforgeryToken] // Apply the attribute to the controller
public class MinioController : ControllerBase
{
    private readonly IMinioClient _minioClient;

    public MinioController(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    [HttpPost("create-bucket/{bucketName}")]
    public async Task<IActionResult> CreateBucket(string bucketName)
    {
        try
        {
            var beArgs = new BucketExistsArgs()
                        .WithBucket(bucketName);

            bool found = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(true);

            if (!found)
            {
                var mbArgs = new MakeBucketArgs()
                            .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(true);

                return Ok($"Bucket {bucketName} created successfully");
            }

            // Fetch details about the contents of the bucket
            var objectStats = new List<string>();

            var listArgs = new ListObjectsArgs()
               .WithBucket(bucketName)
               .WithRecursive(true);

            var observable = _minioClient.ListObjectsAsync(listArgs);


            var subscription = observable.Subscribe(
                               item => objectStats.Add($"{item.Key} (Size: {item.Size} Bytes, Last Modified: {item.LastModified})"),
                                              ex => Console.WriteLine($"OnError: {ex.Message}"),
                                                             () => Console.WriteLine("OnComplete: All items enumerated")
                                                                        );
            return Ok(objectStats);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost("upload-file/{bucketName}")]
    public async Task<IActionResult> UploadFile(string bucketName, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is required.");
        }

        var tempFilePath = Path.GetTempFileName();
        try
        {
            var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucketName);
            bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs).ConfigureAwait(true);
            if (!found)
            {
                return NotFound($"Bucket '{bucketName}' does not exist.");
            }

            await using (var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Write))
            {
                await file.CopyToAsync(fileStream);
            }

            var args = new PutObjectArgs()
                            .WithBucket(bucketName)
                            .WithObject(file.FileName)
                            .WithContentType(file.ContentType)
                            .WithFileName(tempFilePath);

            await _minioClient.PutObjectAsync(args).ConfigureAwait(true);


            return Ok($"File '{file.FileName}' uploaded successfully to bucket '{bucketName}'.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
        finally
        {
            if (System.IO.File.Exists(tempFilePath))
            {
                try
                {
                    System.IO.File.Delete(tempFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting temp file '{tempFilePath}': {ex.Message}");
                }
            }
        }

    }
}
