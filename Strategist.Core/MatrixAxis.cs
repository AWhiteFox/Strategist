using System.Collections.Generic;

namespace Strategist.Core
{
    public class MatrixAxis
    {
        private readonly Dictionary<string, bool> tagsDict;
        private readonly string[] tags;

        public IReadOnlyList<string> Tags => tags;

        public bool IsEnabled
        {
            get
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    if (!tagsDict[tags[i]])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public MatrixAxis(string[] tags, Dictionary<string, bool> tagsDict)
        {
            this.tags = tags;
            this.tagsDict = tagsDict;
        }
    }
}
