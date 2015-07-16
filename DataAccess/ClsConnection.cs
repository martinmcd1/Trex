using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using ErrorHandling;
using TrexV2.Models;

namespace TrexV2.DataAccess
{
    public interface IConnection
    {
        DataTable SelectData(string sql1);
        ObservableCollection<clsData> GetSupportData();
        ObservableCollection<ClsRecords> GetDisplayData();
        void InsertTvp<T>(string storedProcedure, T table);
        string GetConnectionString();

    }

    public class ClsConnection : IConnection
    {
      
        public string ConnectionString;
        private SqlTransaction Transaction { get; set; }


     

        #region Methods

        public DataTable SelectData(string sql1)
        {
            ConnectionString = GetConnectionString();
            using (var conn = new SqlConnection(ConnectionString))
            {
                var dt = new DataTable();
                try
                {
                    conn.Open();
                    Transaction = conn.BeginTransaction();
                    var cmd = new SqlCommand(sql1, conn) { CommandType = CommandType.StoredProcedure };
                    var da = new SqlDataAdapter(cmd);
                    cmd.Transaction = Transaction;
                    Transaction.Commit();
                    da.Fill(dt);
                    return dt;
                }

                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Data Capture", MessageBoxButton.OK, MessageBoxImage.Error);
                    string errorMessage = ErrorLog.CreateErrorMessage(err);
                    ErrorLog.LogFileWriter(errorMessage);
                    Transaction.Rollback();
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public void InsertTvp<T>(string storedProcedure, T table)
        {
            ConnectionString = GetConnectionString();
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    var cmd = new SqlCommand(storedProcedure, conn) {CommandType = CommandType.StoredProcedure};
                    var param = cmd.Parameters.AddWithValue("@TempTable", table);
                    param.SqlDbType = SqlDbType.Structured;
                    cmd.ExecuteNonQuery();
                    
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Data Capture", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        public ObservableCollection<clsData> GetSupportData()
        {
            var dt = SelectData("qrySupport");
            if (dt == null) return null;
            var supportList = (from DataRow dr in dt.Rows
                select new clsData {LiveSupport = dr["Support"].ToString()}).ToList();

            var supportData = new ObservableCollection<clsData>(supportList);
            return supportData;
        }

        public ObservableCollection<ClsRecords> GetDisplayData()
        {
            var dt = SelectData("qryGetRecords");
            if (dt == null) return null;
            var recordsList = (from DataRow dr in dt.Rows
                               select new ClsRecords
                               {
                                   Pid = dr["fldPid"].ToString(),
                                   Support = dr["fldSupport"].ToString(),
                                   SupportDesc = dr["fldSupportDesc"].ToString(),
                                   Start = dr["fldStart"].ToString(),
                                   End = dr["fldEnd"].ToString(),
                                   Total = dr["fldTotal"].ToString()
                               }).ToList();

            var displayData = new ObservableCollection<ClsRecords>(recordsList);
            return displayData;
        }


        public string GetConnectionString()
        {
            var myConnectionString = new SqlConnectionStringBuilder {DataSource = @"MARTINS\MARTINSQLSERVER", InitialCatalog = "Trex", IntegratedSecurity = true};
            return myConnectionString.ConnectionString;
        }
        #endregion ///Methods
    }
}