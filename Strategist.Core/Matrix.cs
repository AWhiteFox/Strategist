using System;
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
        private readonly Pair<Dictionary<int, int>> headerToIndex;
        private readonly Pair<bool> hasCombinedHeaders;

        public int Width => values.Count > 0 ? values[0].Count : 0;
        public int Height => values.Count;
        public IReadOnlyList<IReadOnlyList<string>> ColumnHeaders => headers[0];
        public IReadOnlyList<IReadOnlyList<string>> RowHeaders => headers[1];
        public IReadOnlyList<bool> ColumnsEnabled => headersEnabled[0];
        public IReadOnlyList<bool> RowsEnabled => headersEnabled[1];
        public IReadOnlyDictionary<string, bool> ColumnTags => tagsEnabled[0];
        public IReadOnlyDictionary<string, bool> RowTags => tagsEnabled[1];

        public bool HasCombinedColumnHeaders => hasCombinedHeaders[0];
        public bool HasCombinedRowHeaders => hasCombinedHeaders[1];

        public double this[int i, int j]
        {
            get => values[j][i];
            set => values[j][i] = value;
        }

        public double this[IEnumerable<string> columnHeaders, IEnumerable<string> rowHeaders]
        {
            get => this[GetColumnIndex(columnHeaders), GetRowIndex(rowHeaders)];
            set => this[GetColumnIndex(columnHeaders), GetRowIndex(rowHeaders)] = value;
        }

        public Matrix()
        {
            values = new List<List<double>>();
            headers = Pair.FromFunc(_ => new List<string[]>());
            headersEnabled = Pair.FromFunc(_ => new List<bool>());
            tagsEnabled = Pair.FromFunc(_ => new Dictionary<string, bool>());
            headerToIndex = Pair.FromFunc(_ => new Dictionary<int, int>());
            hasCombinedHeaders = Pair.FromFunc(_ => false);
        }

        public void SetColumnTagEnabled(string key, bool value) => SetAxisTagEnabled(0, key, value);

        public void SetRowTagEnabled(string key, bool value) => SetAxisTagEnabled(1, key, value);

        public void AddColumn(IEnumerable<string> header) => AddAxis(0, header);

        public void AddRow(IEnumerable<string> header) => AddAxis(1, header);

        public int GetColumnIndex(IEnumerable<string> header) => GetAxisIndex(0, header);

        public int GetRowIndex(IEnumerable<string> header) => GetAxisIndex(1, header);
        
        public bool ContainsColumn(IEnumerable<string> header) => ContainsAxis(0, header);

        public bool ContainsRow(IEnumerable<string> header) => ContainsAxis(1, header);

        private void AddAxis(int dim, IEnumerable<string> header)
        {
            string[] tags = header.Distinct().ToArray();
            if (ContainsAxis(dim, tags))
                throw new ArgumentException("An element with the same header already exists.");

            if (tags.Length > 1)
                hasCombinedHeaders[dim] = true;
            
            headers[dim].Add(tags);
            foreach (string t in tags)
            {
                if (!tagsEnabled[dim].ContainsKey(t))
                {
                    tagsEnabled[dim].Add(t, true);
                }
            }
            headersEnabled[dim].Add(tags.All(tag => tagsEnabled[dim][tag]));
            headerToIndex[dim].Add(GetHeaderHashCode(tags), headerToIndex[dim].Count);
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

        private int GetAxisIndex(int dim, IEnumerable<string> header) => headerToIndex[dim].TryGetValue(GetHeaderHashCode(header), out int value) ? value : -1;

        private bool ContainsAxis(int dim, IEnumerable<string> header) => headerToIndex[dim].ContainsKey(GetHeaderHashCode(header));

        private static int GetHeaderHashCode(IEnumerable<string> header) => header.Aggregate(0, (current, element) => current ^ element.GetHashCode());
    }
}
