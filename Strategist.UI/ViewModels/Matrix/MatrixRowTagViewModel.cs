using Strategist.Core;
using Strategist.UI.Abstractions;

namespace Strategist.UI.ViewModels
{
    public class MatrixRowTagViewModel : NotifyPropertyChangedBase
    {
        private readonly Matrix matrix;

        public string Title { get; }
        public bool IsEnabled
        {
            get => matrix.RowTags[Title];
            set
            {
                matrix.SetRowTagEnabled(Title, value);
                NotifyThisPropertyChanged();
            }
        }

        public MatrixRowTagViewModel(string title, Matrix matrix)
        {
            Title = title;
            this.matrix = matrix;
        }
    }
}
