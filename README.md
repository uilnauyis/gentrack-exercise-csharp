### Gentrack Coding Exercise

This is a small program implemented in C# and designed to run on .NET Core to 
Extract data from XML files in a certain format to one or multiple CVS files.

## Requirement
.NET Core 3.1

## To Run this Program
First, set the working directory to the package path.

Then, compile the projects via the following command:
$dotnet build GentrackExercise.sln$

Then the program could be run via:
$dotnet  ./GentrackExercise/bin/Debug/netcoreapp3.1/GentrackExercise.dll {path-to-XML-file}$

If the run was successful, CVS files containing the data will be generated in the 'output' directory in the root directory

## To Run the automated test
At the root directory, first

$cd ./GentrackExercise.Test$

Then run

$dotnet test$