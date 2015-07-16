using TrexV2.DataAccess;
using TrexV2.Models;
using TrexV2.Services;
using TrexV2.ViewModels;

namespace TrexV2
{
    public class Bootstrapper
    {
        public static void Initialize()
        {
            //initialize all the services needed for dependency injection
            //we use the unity framework for dependency injection, but you can choose others



            ServiceProvider.RegisterServiceLocator(new UnityServiceLocator());


            ServiceProvider.Instance.Register<IConnection, ClsConnection>();

            ServiceProvider.Instance.Register<IExportToExcel, ExportToExcel>();

           ServiceProvider.Instance.Resolve<MainViewModel>();
           

        }
    }
}
