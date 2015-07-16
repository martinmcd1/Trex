using System;
using System.Collections.ObjectModel;
using TrexV2.DataAccess;
using TrexV2.Models;

namespace TrexV2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        private readonly ObservableCollection<ViewModelBase> _settings;


        public ObservableCollection<ViewModelBase> Settings
        {
            get { return _settings; }
        }

        public MainViewModel(IConnection conn, IExportToExcel export)
        {
            _settings = new ObservableCollection<ViewModelBase>
            {
                new TeamRecordsViewModel(conn,export),
                new AdminViewModel()
            };
        }



        public override string Name
        {
            get { throw new NotImplementedException(); }
        }
    }
}
