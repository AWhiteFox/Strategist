using Strategist.Core;
using Strategist.UI.Abstractions;
using System;
using System.ComponentModel;

namespace Strategist.UI.ViewModels
{
    public class MatrixRowViewModel : NotifyPropertyChangedBase
    {
        private readonly Matrix matrix;
        private readonly int index;

        public string Header => string.Join(", ", matrix.Rows[index].Tags);
        public bool IsEnabled => matrix.Rows[index].IsEnabled;
        public double this[int i] => matrix[i, index];

        public MatrixRowViewModel(Matrix matrix, int index, MatrixRowTagViewModel[] tagsArr)
        {
            this.matrix = matrix;
            this.index = index;
            var tags = matrix.Rows[index].Tags;
            for (int i = 0; i < tags.Count; i++)
            {
                Array.Find(tagsArr, x => x.Title == tags[i]).PropertyChanged += OnTagIsEnabledChanged;
            }
        }

        private void OnTagIsEnabledChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MatrixRowTagViewModel.IsEnabled))
            {
                NotifyPropertyChanged(nameof(IsEnabled));
            }
        }
    }
}
