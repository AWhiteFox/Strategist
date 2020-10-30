namespace Strategist.Core
{
    public class MatrixColumnRowData : NotifyPropertyChangedBase
    {
        private string header;
        private bool enabled;

        public string Header
        {
            get => header;
            set
            {
                header = value;
                NotifyPropertyChanged();
            }
        }

        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                NotifyPropertyChanged();
            }
        }
    }
}
