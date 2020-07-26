using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Collections;
using System;
using System.Collections.Generic;

namespace gentrack_exercise
{
    class XmlProcessor
    {
        private List<Tuple<string, int>> nodeStack = new List<Tuple<string, int>>();

        internal async Task TestReader(string filePath)
        {
            using (FileStream stream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Async = true;

                using (XmlReader reader = XmlReader.Create(stream, settings))
                {
                    while (await reader.ReadAsync())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            int currentDepth = reader.Depth;
                            while (nodeStack.Count > 0 &&
                                nodeStack[nodeStack.Count - 1].Item2 >= currentDepth)
                            {
                                nodeStack.RemoveAt(nodeStack.Count - 1);
                            }
                            nodeStack.Add(
                                new Tuple<string, int>(reader.Name, reader.Depth));

                            if (reader.Name == "CSVIntervalData" &&
                                verifyTargetPath())
                            {
                                var targetData = await reader.ReadInnerXmlAsync();
                                generateCsvOutput(targetData);
                                return;
                            }
                        }
                    }
                }
            }
        }

        private bool verifyTargetPath() {
            return nodeStack.Count >= 4 &&
                nodeStack[nodeStack.Count - 1].Item1 == "CSVIntervalData" &&
                nodeStack[nodeStack.Count - 2].Item1 == "MeterDataNotification" &&
                nodeStack[nodeStack.Count - 3].Item1 == "Transaction" &&
                nodeStack[nodeStack.Count - 4].Item1 == "Transactions";
        }

        private void generateCsvOutput(string generateCsvOutput)
        {
            throw new NotImplementedException();
        }
    }
}