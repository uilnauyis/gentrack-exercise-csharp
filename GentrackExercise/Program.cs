using System;
using System.IO;
using System.Xml;
using System.Threading.Tasks;

namespace GentrackExercise
{
    public class Program
    {
        const string instructionMessage = "Please provide a vallid path to the" + 
            "'XML' +  file in the terminal.";
        
        const string exitProgramMessage = "Press any key to exit the program";

        public static async Task Main(string[] args)
        {
            if (args.Length < 1) {
                Console.WriteLine("The path to the xml file is not provided");
                Console.WriteLine(exitProgramMessage);
                return;
            }

            string input = args[0];
            if (input == null || !File.Exists(input)) {
                Console.WriteLine("Invalid Path");
                Console.WriteLine(exitProgramMessage);
                return;
            }
            
            XmlProcessor processor = new XmlProcessor();
            await processor.ProcessXml(input);
        }
    }
}
