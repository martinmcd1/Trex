using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TrexV2.ViewModels
{
    public class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion ///INotifyPropertyChanged
    }
}