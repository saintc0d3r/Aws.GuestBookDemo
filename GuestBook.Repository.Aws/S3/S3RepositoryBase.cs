using System.Linq;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace GuestBook.Repository.Aws.S3
{
    abstract class S3RepositoryBase
    {
        private readonly string _bucketName;
        private readonly S3Region _region;

        protected S3RepositoryBase(RegionEndpoint regionEndpoint, string bucketName)
        {
            _bucketName = bucketName;
            S3 = new AmazonS3Client(regionEndpoint);
            _region = regionEndpoint.AsS3Region();
            initialiseBucket();
        }

        private void initialiseBucket()
        {
            var response = S3.ListBuckets();
            if (!response.Buckets.Any(bucket => bucket.BucketName.Equals(_bucketName)))
            {
                S3.PutBucket(new PutBucketRequest { BucketName = _bucketName, BucketRegion = _region });
            }            
        }

        protected AmazonS3Client S3 { private set; get; }
    }
}
