using Microsoft.Practices.Unity;
using TrexV2.ViewModels;

namespace TrexV2.Services
{
    class UnityServiceLocator: IServiceLocator
    {
        private readonly UnityContainer _container;

        public UnityServiceLocator()
        {
            _container = new UnityContainer();
        }

        void IServiceLocator.Register<TInterface, TImplementation>()
        {
            _container.RegisterType<TInterface, TImplementation>();
            
        }

        TInterface IServiceLocator.Get<TInterface>()
        {
            return _container.Resolve<TInterface>();
        }

        void IServiceLocator.Resolve<TImplementation>()
        {
            _container.Resolve<TImplementation>();

        }
    }
}
