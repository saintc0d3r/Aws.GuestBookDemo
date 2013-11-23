using System.Configuration;
using Amazon;
using Amazon.S3;

namespace GuestBook.Repository.Aws
{
    static class CommonHelpers
    {
        public static RegionEndpoint GetRegionEndpointFromConfig()
        {
            var awsRegionSettings = ConfigurationManager.AppSettings["AWSRegion"];

            switch (awsRegionSettings)
            {
                case "ap-southeast-1":
                    return RegionEndpoint.APSoutheast1;
                case "ap-southeast-2":
                    return RegionEndpoint.APSoutheast2;
                case "ap-northeast-1":
                    return RegionEndpoint.APNortheast1;
                case "us-east-1":
                    return RegionEndpoint.USEast1;
                case "us-west-1":
                    return RegionEndpoint.USWest1;
                case "us-west-2":
                    return RegionEndpoint.USWest2;
                case "eu-west-1":
                    return RegionEndpoint.EUWest1;
                case "sa-east-1":
                    return RegionEndpoint.SAEast1;
            }

            return RegionEndpoint.APSoutheast1;
        }

        public static S3Region AsS3Region(this RegionEndpoint regionEndpoint)
        {
            if (regionEndpoint == RegionEndpoint.APNortheast1)
            {
                return S3Region.APN1;
            }
            if (regionEndpoint == RegionEndpoint.APSoutheast1)
            {
                return S3Region.APS1;
            }
            if (regionEndpoint == RegionEndpoint.APSoutheast2)
            {
                return S3Region.APS2;
            }
            if (regionEndpoint == RegionEndpoint.EUWest1)
            {
                return S3Region.EU;
            }
            if (regionEndpoint == RegionEndpoint.SAEast1)
            {
                return S3Region.SAE1;
            }
            if (regionEndpoint == RegionEndpoint.USEast1)
            {
                return S3Region.US;
            }
            if (regionEndpoint == RegionEndpoint.USWest1)
            {
                return S3Region.USW1;
            }
            if (regionEndpoint == RegionEndpoint.USWest2)
            {
                return S3Region.USW2;
            }

            return S3Region.SFO;
        }

    }
}
