using Strategist.Core.Abstractions;

namespace Strategist.Core
{
    public class MatrixHeader : NotifyPropertyChangedBase
    {
        private bool enabled = true;

        public string Title { get; }
        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    NotifyPropertyChanged(nameof(Enabled));
                }
            }
        }

        public MatrixHeader(string title)
        {
            Title = title;
        }
    }
}
