### Gentrack Coding Exercise

This is a small program implemented in C# and designed to run on .NET Core to 
Extract data from XML files in a certain format to one or multiple CVS files.

## Requirement
.NET Core 3.1

## To Run this Program
First, set the working directory to the package path.

Then, compile the projects via the following command:
```dotnet build GentrackExercise.sln```

Then the program could be run via:
```dotnet  ./GentrackExercise/bin/Debug/netcoreapp3.1/GentrackExercise.dll {path-to-XML-file}```

If the run was successful, CVS files containing the data will be generated in the 'output' directory in the root directory

## To Run the automated test
At the root directory, first

```cd ./GentrackExercise.Test```

Then run

```dotnet test```


## What I have done
I implemented the program and added a single test case to verify that the ouput is exactly the same as reference files.

## What to do next if given more time
1. Improve test coverage, including
   * Test the code with much larger xml files. I have consider this, so I implemented the file reading using stream. Yet it still need more tests.
   * Test the code with data element that in incorrect path. For example, a 'CSVIntervalData' may not satisfy ' Transactions->Transaction->MeterDataNotification->CSVIntervalData' order. I work with this case using a stack to store the path.
   * Test with large element. This need more investigation

2. Add doc comment to all the public methods

3. Add a GUI interface to make it easier for user to use and easier making validate input.