using System;
using System.Collections.Generic;
using System.Linq;

namespace Strategist.Core
{
    public class Matrix
    {
        private readonly List<List<double>> values;
        private readonly List<MatrixAxis> columns;
        private readonly List<MatrixAxis> rows;
        private readonly Dictionary<string, bool> columnTags;
        private readonly Dictionary<string, bool> rowTags;

        public IReadOnlyDictionary<string, bool> ColumnTags => columnTags;
        public IReadOnlyDictionary<string, bool> RowTags => rowTags;
        public IReadOnlyList<MatrixAxis> Columns => columns;
        public IReadOnlyList<MatrixAxis> Rows => rows;

        public double this[int i, int j]
        {
            get => values[j][i];
            set => values[j][i] = value;
        }

        public Matrix()
        {
            values = new List<List<double>>();
            columns = new List<MatrixAxis>();
            rows = new List<MatrixAxis>();
            columnTags = new Dictionary<string, bool>();
            rowTags = new Dictionary<string, bool>();
        }

        public void SetColumnTagEnabled(string key, bool value) => columnTags[key] = value;

        public void SetRowTagEnabled(string key, bool value) => rowTags[key] = value;

        public void AddColumn(string[] tags)
        {
            AddAxis(columns, tags, columnTags);
            for (int i = 0; i < values.Count; i++)
            {
                values[i].Add(0.0);
            }
        }

        public void AddRow(string[] tags)
        {
            AddAxis(rows, tags, rowTags);
            values.Add(Enumerable.Repeat(0.0, columns.Count).ToList());
        }

        private static void AddAxis(List<MatrixAxis> axisCollection, string[] tags, Dictionary<string, bool> tagsDict)
        {
            for (int i = 0; i < tags.Length; i++)
            {
                if (!tagsDict.ContainsKey(tags[i]))
                {
                    tagsDict.Add(tags[i], true);
                }
            }
            axisCollection.Add(new MatrixAxis(tags, tagsDict));
        }
    }
}
