using System;
using System.ComponentModel;

namespace TrexV2.ViewModels
{
    public abstract class ViewModelBase : ObservableObject, IDataErrorInfo
    {
        public ViewModelBase ViewModel { get; set; }

        public abstract string Name { get; }

        public string this[string columnName]
        {
            get { return OnValidate(columnName); }
        }

        [Obsolete]
        public string Error
        {
            get { throw new NotSupportedException(); }
        }

        public virtual string OnValidate(string propertyName)
        {
            return null;
        }


    }
}