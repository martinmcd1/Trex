using System.Windows;
using Microsoft.Practices.Unity;
using TrexV2.DataAccess;
using TrexV2.Models;
using TrexV2.ViewModels;

namespace TrexV2
{

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IExportToExcel, ExportToExcel>();
            container.RegisterType<IConnection, ClsConnection>();
            container.RegisterType<TeamRecordsViewModel>();
            container.RegisterType<IExportToExcel, ExportToExcel>();
            container.RegisterType<IConnection, ClsConnection>();
        //    container.RegisterType<MainViewModel>();
          //Bootstrapper.Initialize();
            //var vm = new MainWindow();
            //vm.Show();
            var main = new MainWindow()
            {
                DataContext = container.Resolve<MainViewModel>()
            };

            main.Show();



        }
    }
}