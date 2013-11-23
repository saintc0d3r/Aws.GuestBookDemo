using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuestBook.Spec
{
    [TestClass]
    public class SpecBootstrap
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext testContext)
        {
            bindingTypes();
        }

        private static void bindingTypes()
        {
            // TODO: Binding types into the injection
            //DependencyInjector.LoadModules(new [] {});
        }
    }
}
