using Strategist.Core;
using Strategist.UI.Abstractions;

namespace Strategist.UI.ViewModels
{
    public class MatrixColumnTagViewModel : NotifyPropertyChangedBase
    {
        private readonly Matrix matrix;

        public string Title { get; }
        public bool IsEnabled
        {
            get => matrix.ColumnTags[Title];
            set
            {
                matrix.SetColumnTagEnabled(Title, value);
                NotifyThisPropertyChanged();
            }
        }

        public MatrixColumnTagViewModel(string title, Matrix matrix)
        {
            Title = title;
            this.matrix = matrix;
        }
    }
}
