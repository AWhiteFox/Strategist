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
        private readonly Pair<Dictionary<int, int>> headersDictionaries;

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

        public double this[IEnumerable<string> columnHeaders, IEnumerable<string> rowHeaders]
        {
            get => values[headersDictionaries[1][GetHeadersHashCode(columnHeaders)]][headersDictionaries[0][GetHeadersHashCode(rowHeaders)]];
            set => values[headersDictionaries[1][GetHeadersHashCode(columnHeaders)]][headersDictionaries[0][GetHeadersHashCode(rowHeaders)]] = value;
        }

        public Matrix()
        {
            values = new List<List<double>>();
            headers = Pair.FromFunc(_ => new List<string[]>());
            headersEnabled = Pair.FromFunc(_ => new List<bool>());
            tagsEnabled = Pair.FromFunc(_ => new Dictionary<string, bool>());
            headersDictionaries = Pair.FromFunc(_ => new Dictionary<int, int>());
        }

        public void SetColumnTagEnabled(string key, bool value) => SetAxisTagEnabled(0, key, value);

        public void SetRowTagEnabled(string key, bool value) => SetAxisTagEnabled(1, key, value);

        public void AddColumn(string[] tags) => AddAxis(0, tags);

        public void AddRow(string[] tags) => AddAxis(1, tags);

        private void AddAxis(int dim, string[] tags)
        {
            headers[dim].Add(tags);
            foreach (string t in tags)
            {
                if (!tagsEnabled[dim].ContainsKey(t))
                {
                    tagsEnabled[dim].Add(t, true);
                }
            }
            headersEnabled[dim].Add(tags.All(tag => tagsEnabled[dim][tag]));
            headersDictionaries[dim].Add(GetHeadersHashCode(tags.Distinct()), headersDictionaries[dim].Count);
            if (dim == 0)
            {
                foreach (var t in values)
                {
                    t.Add(0.0);
                }
            }
            else
            {
                values.Add(Enumerable.Repeat(0.0, headers[0].Count).ToList());
            }
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

        private static int GetHeadersHashCode(IEnumerable<string> headers)
        {
            return headers.Aggregate(0, (current, element) => current ^ element.GetHashCode());
        }
    }
}
