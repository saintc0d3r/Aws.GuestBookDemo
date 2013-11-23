using GuestBook.Domain.Services;

using Ninject.Modules;

namespace GuestBook.Service
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IGuestBookService>().To<GuestBookService>();
        }
    }
}
