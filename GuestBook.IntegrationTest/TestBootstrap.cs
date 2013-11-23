using GuestBook.Repository.Aws;
using GuestBook.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.Modules;
using Xtremecode.Infrastructure.Di;

namespace GuestBook.IntegrationTest
{
    [TestClass]
    public class TestBootstrap
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext testContext)
        {
            bindingTypes();
        }

        private static void bindingTypes()
        {
            DependencyInjector.LoadModules(new NinjectModule[]{ new ServiceModule(), new RepositoryAwsModule() });
        }
    }
}
