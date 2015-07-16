using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrexV2.Services
{
    public interface IServiceLocator
    {
        void Register<TInterface, TImplementation>() where TImplementation : TInterface;

        TInterface Get<TInterface>();

        void Resolve<TImplementation>();


    }

    class ServiceProvider
    {
        public static IServiceLocator Instance { get; private set; }

        public static void RegisterServiceLocator(IServiceLocator s)
        {
            Instance = s;
        }
    }
}
