using Strategist.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategist.UI.ViewModels
{
    public abstract class MatrixAxisViewModel
    {
        private readonly Matrix matrix;
        private readonly int index;

        public abstract string Header { get; }
        public abstract bool Enabled { get; }

        public MatrixAxisViewModel(Matrix matrix, int index, MatrixRowTagViewModel[] tagsList)
        {
            this.matrix = matrix;
            this.index = index;
            var tags = matrix.Rows[index].Tags;
            for (int i = 0; i < tags.Count; i++)
            {
                Array.Find(tagsList, x => x.Title == tags[i]).PropertyChanged += OnTagIsEnabledChanged;
            }
        }

        private void OnTagIsEnabledChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MatrixRowTagViewModel.IsEnabled))
            {
                
            }
        }
    }
}
