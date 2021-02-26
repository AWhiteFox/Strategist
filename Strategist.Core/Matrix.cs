using Strategist.Core.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Strategist.Core
{
    public class Matrix
    {
        private readonly List<List<double>> values;
        private readonly Pair<List<string[]>> headers;
        private readonly Pair<List<bool>> headersEnabled;
        private readonly Pair<Dictionary<string, bool>> tagsEnabled;

        public int Width => values.Count > 0 ? values[0].Count : 0;
        public int Height => values.Count;
        public IReadOnlyList<string[]> ColumnHeaders => headers[0];
        public IReadOnlyList<string[]> RowHeaders => headers[1];
        public IReadOnlyList<bool> ColumnsEnabled => headersEnabled[0];
        public IReadOnlyList<bool> RowsEnabled => headersEnabled[1];
        public IReadOnlyDictionary<string, bool> ColumnTags => tagsEnabled[0];
        public IReadOnlyDictionary<string, bool> RowTags => tagsEnabled[1];

        public double this[int i, int j]
        {
            get => values[j][i];
            set => values[j][i] = value;
        }

        public Matrix()
        {
            values = new List<List<double>>();
            headers = Pair.FromFunc(_ => new List<string[]>());
            headersEnabled = Pair.FromFunc(_ => new List<bool>());
            tagsEnabled = Pair.FromFunc(_ => new Dictionary<string, bool>());
        }

        public void SetColumnTagEnabled(string key, bool value) => SetAxisTagEnabled(0, key, value);

        public void SetRowTagEnabled(string key, bool value) => SetAxisTagEnabled(1, key, value);

        public void AddColumn(string[] tags)
        {
            AddAxis(0, tags);
            for (int i = 0; i < values.Count; i++)
            {
                values[i].Add(0.0);
            }
        }

        public void AddRow(string[] tags)
        {
            AddAxis(1, tags);
            values.Add(Enumerable.Repeat(0.0, headers[0].Count).ToList());
        }

        private void AddAxis(int dim, string[] tags)
        {
            headers[dim].Add(tags);
            for (int i = 0; i < tags.Length; i++)
            {
                if (!tagsEnabled[dim].ContainsKey(tags[i]))
                {
                    tagsEnabled[dim].Add(tags[i], true);
                }
            }
            headersEnabled[dim].Add(tags.All(tag => tagsEnabled[dim][tag]));
        }

        private void SetAxisTagEnabled(int dim, string key, bool value)
        {
            if (tagsEnabled[dim][key] == value)
                return;

            tagsEnabled[dim][key] = value;
            for (int i = 0; i < headers[dim].Count; i++)
            {
                headersEnabled[dim][i] = headers[dim][i].All(tag => tagsEnabled[dim][tag]);
            }
        }
    }
}
