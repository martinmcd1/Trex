using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TrexV2.DataAccess;
using TrexV2.Models;

namespace TrexV2.ViewModels
{
    public class TeamRecordsViewModel : ViewModelBase
    {
        #region Private members

        private readonly IConnection _connection;
        private readonly IExportToExcel _export;
        private ObservableCollection<clsData> _liveSupport;
        private ObservableCollection<ClsRecords> _showRecords;
        private clsData _selectedSupport;
        private ICommand _submitData;
        private string _supportText;
        private string _endTime;
        private string _startTime;
        private string _totalTime;
        private bool _textIsEnbaled;

        #endregion ///Private members

        #region Constructor

        public TeamRecordsViewModel(IConnection conn, IExportToExcel export)
        {
            _connection = conn;
            _export = export;
            _liveSupport = _connection.GetSupportData();
            ShowRecords = _connection.GetDisplayData();
        }


        #endregion /// Constructor

        #region Commands

        public ICommand SubmitData
        {
            get
            {
                return _submitData ?? (_submitData = new RelayCommand(param => SaveTeamRecordsTvp(), param => CanSave));
            }
        }

        #endregion ///Commands



        #region Properties

        public override string Name
        {
            get { return "Team Records"; }
        }

        public ObservableCollection<clsData> LiveSupport
        {
            get { return _liveSupport; }
            set
            {
                _liveSupport = value;
                NotifyPropertyChanged();
            }
        }

        public clsData SelectedSupport
        {
            get { return _selectedSupport; }
            set
            {
                _selectedSupport = value;
                NotifyPropertyChanged();
                TextIsEnabled = SelectedSupport != null;
            }
        }

        public ObservableCollection<ClsRecords> ShowRecords
        {
            get { return _showRecords; }
            set
            {
                _showRecords = value;
                NotifyPropertyChanged();
            }
        }

        public string SupportText
        {
            get { return _supportText; }
            set
            {
                _supportText = value;
                NotifyPropertyChanged();
            }
        }

        public string StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                NotifyPropertyChanged();
            }
        }

        public string EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                NotifyPropertyChanged();
            }
        }

        public bool TextIsEnabled
        {
            get { return _textIsEnbaled; }
            set
            {
                _textIsEnbaled = value;
                NotifyPropertyChanged();
            }
        }
        
        protected bool CanSave { get; private set; }
        #endregion ///Properties

        #region Methods

         public override string OnValidate(string propertyName)
        {
            if (propertyName == "StartTime")
            {
                if (!string.IsNullOrWhiteSpace(StartTime))
                {
                    var regex = new Regex(@"([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
                    var match = regex.Match(StartTime);
                    return match.Success ? null : "Not Valid Time";
                }
            }


            if (propertyName != "EndTime") return null;
            if (string.IsNullOrWhiteSpace(EndTime) || string.IsNullOrWhiteSpace(StartTime)) return null;
            var regexEnd = new Regex(@"([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
            var matchEnd = regexEnd.Match(EndTime);
            if (matchEnd.Success)
            {
                var start = Convert.ToDateTime(StartTime);
                var end = Convert.ToDateTime(EndTime);
                if (start > end)
                {
                    CanSave = false;
                    return "Start time cannot be before End Time";
                }
                CanSave = true;
                return null;
            }
            CanSave = false;
            return "Not Valid Time";
        }



        public void SaveTeamRecordsTvp()
        {
            var datatable = CreateDataTable();
            var pid = Environment.GetEnvironmentVariable("Username");
            var theDate = DateTime.Now;
            var date = theDate.ToString("d");
            var time = theDate.ToString("T");
            var id = pid + date + time;
            var start = Convert.ToDateTime(StartTime);
            var end = Convert.ToDateTime(EndTime);
            _totalTime = TimeCalc(start,end);
            datatable.Rows.Add(id,pid, SelectedSupport.LiveSupport, SupportText, StartTime, EndTime, _totalTime);
 
            _export.ExcelFromDataTable(datatable, "Test");
            _connection.InsertTvp("InsertRecords", datatable);
            ShowRecords = _connection.GetDisplayData();
            FormReset();
        }

        public DataTable CreateDataTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("fldID");
            dt.Columns.Add("fldPid");
            dt.Columns.Add("fldSupport");
            dt.Columns.Add("fldSupportDesc");
            dt.Columns.Add("fldStart");
            dt.Columns.Add("fldEnd");
            dt.Columns.Add("fldTotal");
            return dt;

        }

        public string TimeCalc(DateTime start, DateTime end)
        {
            var total = end - start;

            return total.ToString();
        }


        //resets the user input form.
        public void FormReset()
        {
            if (SupportText != null)
                SupportText = string.Empty;
            if (StartTime != null)
                StartTime = String.Empty;
            if (EndTime != null)
                EndTime = String.Empty;
            SelectedSupport = null;
        }

        #endregion ///Methods
    }
}