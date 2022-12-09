using Amazon.S3;
using Amazon.S3.Transfer;

namespace CulinaCloud.Web.BFF.APIGateway.Controllers
{
    [Route("images")]
    public class ImagesController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IOptions<ImagesServiceSettings> _settings;

        private readonly string[] _permittedExtensions = {
            ".png", ".jpeg", ".jpg", ".svg"
        };
 
        public ImagesController(ICurrentUserService currentUserService, IOptions<ImagesServiceSettings> settings)
        {
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }


        [HttpPost("upload")]
        [Authorize("create:image")]
        public async Task<ActionResult> UploadImage(IFormFile image)
        {
            var s3 = new AmazonS3Client();
            var fileTransferUtility = new TransferUtility(s3);
            var bucketName = _settings.Value.BucketName;
            var distributionDomainName = _settings.Value.DistibutionOriginName;

            var uploadedImages = new Dictionary<string, string>();
            if (image.Length <= 0)
            {
                return BadRequest("Image Size Not Permitted");
            }
            var trustedFileNameForDisplay = WebUtility.HtmlEncode(image.FileName);
            var ext = Path.GetExtension(trustedFileNameForDisplay).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !_permittedExtensions.Contains(ext))
            {
                return BadRequest("Image Type Not Permitted");
            }
            var trustedFileNameForFileStorage = Guid.NewGuid().ToString();

            var key = $"recipes/{trustedFileNameForFileStorage}{ext}";
            var publicUrl = $"https://{distributionDomainName}/{key}";


            await using var readStream = image.OpenReadStream();
            var request = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = readStream
            };
            request.Metadata.Add("Uploader", _currentUserService.UserId);
            request.Metadata.Add("DisplayName", trustedFileNameForDisplay);
            request.Metadata.Add("PublicUrl", publicUrl);
            await fileTransferUtility.UploadAsync(request);


            uploadedImages.Add(trustedFileNameForDisplay, publicUrl);

            return Ok(uploadedImages);
        }
    }
}
