using Strategist.Core.Abstractions;
using System.Linq;

namespace Strategist.Core
{
    public class MatrixRow : NotifyPropertyChangedBase
    {
        public Matrix matrix;
        private readonly int index;

        public MatrixHeader[] Headers { get; }

        public string FullHeader => string.Join(", ", Headers.Select(x => x.Title));

        public bool Enabled
        {
            get
            {
                for (int i = 0; i < Headers.Length; i++)
                {
                    if (!Headers[i].Enabled)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public double this[int i] => matrix.Values[i, index];

        public MatrixRow(Matrix matrix, int index, MatrixHeader[] headers)
        {
            this.matrix = matrix;
            this.index = index;
            Headers = headers;

            for (int i = 0; i < Headers.Length; i++)
            {
                Headers[i].PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(MatrixHeader.Enabled))
                    {
                        NotifyPropertyChanged(nameof(Enabled));
                    }
                };
            }
        }
    }
}
