using GuestBook.Repository.Aws;
using GuestBook.Service;
using Ninject.Modules;
using Xtremecode.Infrastructure.Di;
using Xtremecode.Infrastructure.WebMvc;

namespace GuestBook.Web.Controllers
{
    public class ControllerFactory : ControllerFactoryBase
    {
        protected override void loadModules()
        {
            // DependencyInjector.LoadModules(new NinjectModule[] { new DataAccessModule(), new ServiceModule() });
            DependencyInjector.LoadModules(new NinjectModule[] { new ServiceModule(), new RepositoryAwsModule() });
        }
    }
}
