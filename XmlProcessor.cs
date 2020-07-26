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
                            // Maintain current path in the stack ('nodeStack')
                            int currentDepth = reader.Depth;
                            while (nodeStack.Count > 0 &&
                                nodeStack[nodeStack.Count - 1].Item2 >= currentDepth)
                            {
                                nodeStack.RemoveAt(nodeStack.Count - 1);
                            }

                            // 'Transacion' element should has "transactionDate"
                            // and "transactionID" elements, or will be ignored
                            // and not added to the stack. 
                            if (reader.Name == "Transaction" &&
                                (reader.GetAttribute("transactionDate") == null ||
                                reader.GetAttribute("transactionID") == null))
                            {
                                continue;
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

        private bool verifyTargetPath()
        {
            return nodeStack.Count >= 4 &&
                nodeStack[nodeStack.Count - 1].Item1 == "CSVIntervalData" &&
                nodeStack[nodeStack.Count - 2].Item1 == "MeterDataNotification" &&
                nodeStack[nodeStack.Count - 3].Item1 == "Transaction" &&
                nodeStack[nodeStack.Count - 4].Item1 == "Transactions";
        }

        private void generateCsvOutput(string data)
        {
            string[] rows = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string header = null;
            string outputBasePath = "./output/";
            string filepath = null;
            bool fileEnded = true;
            for (int index = 0; index < rows.Length; index++)
            {
                string row = rows[index];
                string[] items = row.Split(',');

                if (items[0] == "100")
                {
                    header = row;
                }
                else if (items[0] == "200")
                {
                    if (!fileEnded)
                    {
                        using (StreamWriter writer =
                            new StreamWriter(
                                new FileStream(filepath,
                                    FileMode.Append,
                                    FileAccess.Write)))
                        {
                            writer.WriteLine("900");
                        }
                    }
                    fileEnded = false;
                    Directory.CreateDirectory(outputBasePath);
                    filepath = $"{outputBasePath}{items[1]}.csv";
                    using (StreamWriter writer = new StreamWriter(
                        new FileStream(filepath,
                                        FileMode.Create,
                                        FileAccess.Write)))
                    {
                        writer.WriteLine(header);
                        writer.WriteLine(row);
                    }
                }
                else if (items[0] == "300" || items[0] == "900")
                {
                    if (items[0] == "900")
                    {
                        fileEnded = true;
                    }
                    using (StreamWriter writer = new StreamWriter(
                        new FileStream(filepath,
                                        FileMode.Append,
                                        FileAccess.Write)))
                    {
                        writer.WriteLine(row);
                    }
                }
            }
        }
    }
}