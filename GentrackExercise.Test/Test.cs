using Xunit;
using Xunit.Abstractions;
using System.IO;
using System.Threading.Tasks;
using System;
using GentrackExercise;

namespace GentrackExercise.Test
{
    public class Test
    {
        private readonly ITestOutputHelper output;

        public Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ShouldGenerateSameCsvFilesAsSamples()
        {
            output.WriteLine("This is output from {0}", Directory.GetCurrentDirectory());

            const string outputFile1Path = "./output/12345678901.csv";
            const string outputFile2Path = "./output/98765432109.csv";
            const string referenceFile1Path = "../../../testResources/12345678901.csv";
            const string referenceFile2Path = "./../../testResources/98765432109.csv";
            const string testFile = "../../../testResources/testfile.xml";

            Task.Run(() => Program.Main(new string[] { testFile }))
                .Wait();
                
            // CSV files should be generated    
            Assert.True(File.Exists(outputFile1Path),
                $"Program should generate file at Path {Directory.GetCurrentDirectory()}");
            Assert.True(File.Exists(outputFile2Path),
                $"Program should generate file at Path {outputFile2Path}");

            // Generated files should be identical as reference files
            Assert.True(FilesAreIdentical(
                outputFile1Path,
                "../../../testResources/12345678901.csv"),
                $"File {referenceFile1Path} and file {outputFile1Path} should" +
                    "be identical");

            Assert.True(FilesAreIdentical(
                outputFile2Path,
                "../../../testResources/98765432109.csv", ));
        }

        bool FilesAreIdentical(string file1Path, string file2Path)
        {
            int file1Byte;
            int file2Byte;
            FileStream fs1 = new FileStream(file1Path, FileMode.Open, FileAccess.Read);
            FileStream fs2 = new FileStream(file2Path, FileMode.Open, FileAccess.Read);

            // Check the file sizes. If they are not the same, the files 
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1Byte = fs1.ReadByte();
                file2Byte = fs2.ReadByte();
            }
            while ((file1Byte == file2Byte) && (file1Byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return ((file1Byte - file2Byte) == 0);
        }
    }
}