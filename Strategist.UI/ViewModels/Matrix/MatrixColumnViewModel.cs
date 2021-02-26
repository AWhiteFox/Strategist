using Strategist.Core;
using Strategist.UI.Abstractions;
using System;
using System.ComponentModel;

namespace Strategist.UI.ViewModels
{
    public class MatrixColumnViewModel : NotifyPropertyChangedBase
    {
        private readonly Matrix matrix;
        private readonly int index;

        public string Header => string.Join(", ", matrix.ColumnHeaders[index]);
        public bool IsEnabled => matrix.ColumnsEnabled[index];

        public MatrixColumnViewModel(Matrix matrix, int index, MatrixColumnTagViewModel[] tagsArr)
        {
            this.matrix = matrix;
            this.index = index;
            var tags = matrix.ColumnHeaders[index];
            for (int i = 0; i < tags.Length; i++)
            {
                Array.Find(tagsArr, x => x.Title == tags[i]).PropertyChanged += OnTagIsEnabledChanged;
            }
        }

        private void OnTagIsEnabledChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MatrixColumnTagViewModel.IsEnabled))
            {
                NotifyPropertyChanged(nameof(IsEnabled));
            }
        }
    }
}
