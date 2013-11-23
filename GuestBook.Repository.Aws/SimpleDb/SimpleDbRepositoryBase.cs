using System.Linq;
using System.Net;

using Amazon;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;

namespace GuestBook.Repository.Aws.SimpleDb
{
    abstract class SimpleDbRepositoryBase
    {
        private readonly string _domainName;

        protected SimpleDbRepositoryBase(RegionEndpoint regionEndpoint, string domainName) 
        {
            _domainName = domainName;
            SimpleDb = AWSClientFactory.CreateAmazonSimpleDBClient(regionEndpoint);
            initialiseDomain();
        }

        protected SimpleDbRepositoryBase(string domainName) :this( CommonHelpers.GetRegionEndpointFromConfig(), domainName) { }

        private void initialiseDomain()
        {
            // Check the required domain whether it exists or not
            var listDomainsResponse = SimpleDb.ListDomains();
            if ((listDomainsResponse.HttpStatusCode == HttpStatusCode.OK) &&
                 (!listDomainsResponse.DomainNames.Any(domainName => domainName.Equals(_domainName))))
            {
                // Create "GuestBookDemo" domain
                SimpleDb.CreateDomain(new CreateDomainRequest { DomainName = _domainName });
            }
        }

        protected IAmazonSimpleDB SimpleDb { get; private set; }
    }
}
