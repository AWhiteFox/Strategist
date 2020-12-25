using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Strategist.UI.Abstractions
{
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyThisPropertyChanged([CallerMemberName] string propertyName = "") => NotifyPropertyChanged(propertyName);
    }
}
