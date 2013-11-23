// using GuestBook.Repository.Aws.DynamoDb;
using Ninject.Modules;

namespace GuestBook.Repository.Aws
{
    public class RepositoryAwsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IGuestBookEntryRepository>().To<GuestBookEntryRepository>();
            Bind<IGuestBookImageRepository>().To<GuestBookImageRepository>();
        }
    }
}
