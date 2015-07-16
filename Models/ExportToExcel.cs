using System;
using System.Windows;
using ErrorHandling;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;

namespace TrexV2.Models
{

    public interface IExportToExcel
    {
        void ExcelFromDataTable(DataTable dt, string reportName);
    }

  public  class ExportToExcel : IExportToExcel
    {

        #region private members
        Workbook _mWorkBook;
        Sheets _mWorkSheets;
        Application _oXl;
        Worksheet _mWSheet1;
      private const string Path = @"C:\Users\Martin\Documents\Custom Office Templates\book1.xltx";

      #endregion


        #region Methods
        public void ExcelFromDataTable(DataTable dt, string reportName)
        {
            try
            {
                _oXl = new Application {DisplayAlerts = false};
                //opens the required exel document
                _mWorkBook = _oXl.Workbooks.Open(Path, 0, false, 5, "", "", false, XlPlatform.xlWindows, true, false, 0,
                    true, false, false);
                _mWorkSheets = _mWorkBook.Worksheets;
                _mWSheet1 = (Worksheet)_mWorkSheets.Item["sheet1"];

                //add the columns and rows from the datatable to the excel template
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _mWSheet1.Range["A4"].Offset[0, i].Value = dt.Columns[i].ColumnName;
                }
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    _mWSheet1.Range["A5"].Offset[i].Resize[1, dt.Columns.Count].Value = dt.Rows[i].ItemArray;
                }
                //applies the reports name in the template
                _mWSheet1.Cells[2, 1] = reportName;

                //shows the workbook
                _oXl.Visible = true;
                _mWSheet1 = null;
                _mWorkBook = null;

                //clear up operations;
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                var errorMessage = ErrorLog.CreateErrorMessage(err);
                ErrorLog.LogFileWriter(errorMessage);
            }
        }
        #endregion
    }
}
