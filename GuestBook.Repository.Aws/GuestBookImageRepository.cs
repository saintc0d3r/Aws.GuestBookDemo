using Xtremecode.Infrstructure.Aws;
using Xtremecode.Infrstructure.Aws.S3;

namespace GuestBook.Repository.Aws
{
    class GuestBookImageRepository : S3RepositoryBase, IGuestBookImageRepository
    {
        // Adjust the BUCKET_NAME to match yours
        public const string BUCKET_NAME = "guestbookdemo-bucket-aws";

        public GuestBookImageRepository() : base(CommonHelpers.GetRegionEndpointFromConfig(), BUCKET_NAME) { }
    }
}
