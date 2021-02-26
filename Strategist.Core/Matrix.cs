using System;
using System.Collections.Generic;
using System.Linq;

namespace Strategist.Core
{
    public class Matrix
    {
        private readonly List<List<double>> values;
        private readonly List<string[]> columnHeaders;
        private readonly List<string[]> rowHeaders;
        private readonly List<bool> columnsEnabled;
        private readonly List<bool> rowsEnabled;
        private readonly Dictionary<string, bool> columnTags;
        private readonly Dictionary<string, bool> rowTags;

        public IReadOnlyDictionary<string, bool> ColumnTags => columnTags;
        public IReadOnlyDictionary<string, bool> RowTags => rowTags;
        public IReadOnlyList<string[]> ColumnHeaders => columnHeaders;
        public IReadOnlyList<string[]> RowHeaders => rowHeaders;
        public IReadOnlyList<bool> ColumnsEnabled => columnsEnabled;
        public IReadOnlyList<bool> RowsEnabled => rowsEnabled;

        public double this[int i, int j]
        {
            get => values[j][i];
            set => values[j][i] = value;
        }

        public Matrix()
        {
            values = new List<List<double>>();
            columnHeaders = new List<string[]>();
            rowHeaders = new List<string[]>();
            columnTags = new Dictionary<string, bool>();
            rowTags = new Dictionary<string, bool>();

            columnsEnabled = new List<bool>();
            rowsEnabled = new List<bool>();
        }

        public void SetColumnTagEnabled(string key, bool value)
        {
            SetAxisTagEnabled(columnHeaders, columnsEnabled, columnTags, key, value);
        }

        public void SetRowTagEnabled(string key, bool value)
        {
            SetAxisTagEnabled(rowHeaders, rowsEnabled, rowTags, key, value);
        }

        public void AddColumn(string[] tags)
        {
            AddAxis(columnHeaders, columnsEnabled, columnTags, tags);
            for (int i = 0; i < values.Count; i++)
            {
                values[i].Add(0.0);
            } 
        }

        public void AddRow(string[] tags)
        {
            AddAxis(rowHeaders, rowsEnabled, rowTags, tags);
            values.Add(Enumerable.Repeat(0.0, columnHeaders.Count).ToList());
        }

        private static void AddAxis(List<string[]> headersList, List<bool> enabledList, Dictionary<string, bool> tagDictionary, string[] tags)
        {
            headersList.Add(tags);
            for (int i = 0; i < tags.Length; i++)
            {
                if (!tagDictionary.ContainsKey(tags[i]))
                {
                    tagDictionary.Add(tags[i], true);
                }
            }
            enabledList.Add(tags.All(tag => tagDictionary[tag]));
        }

        private static void SetAxisTagEnabled(List<string[]> headersList, List<bool> enabledList, Dictionary<string, bool> tagDictionary, string key, bool value)
        {
            if (tagDictionary[key] == value)
                return;

            tagDictionary[key] = value;
            for (int i = 0; i < headersList.Count; i++)
            {
                enabledList[i] = headersList[i].All(tag => tagDictionary[tag]);
            }
        }
    }
}
