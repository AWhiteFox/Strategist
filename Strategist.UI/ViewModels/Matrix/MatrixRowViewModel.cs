using Strategist.Core;
using Strategist.UI.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Strategist.UI.ViewModels
{
    public class MatrixRowViewModel : NotifyPropertyChangedBase
    {
        private readonly Matrix matrix;
        private readonly int index;

        public string Header => string.Join(", ", matrix.RowHeaders[index]);
        public IReadOnlyList<string> Tags => matrix.RowHeaders[index];
        public bool IsEnabled => matrix.RowsEnabled[index];
        public double this[int i] => matrix[i, index];

        public MatrixRowViewModel(Matrix matrix, int index, MatrixRowTagViewModel[] tagsArr)
        {
            this.matrix = matrix;
            this.index = index;
            var tags = matrix.RowHeaders[index];
            foreach (string t in tags)
            {
                Array.Find(tagsArr, x => x.Title == t).PropertyChanged += OnTagIsEnabledChanged;
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
