using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace Lab_04_Lib
{
    public static class Lab03
    {
        public static void Launch(string inputPath, string outputPath)
        {
            try
            {
                string inputContents = ReadContentsFromFile(inputPath);

                Console.WriteLine("Succesfully read file contents");

                ParticleEnvironment environment = GetParticleEnvironmentFromString(inputContents);

                Console.WriteLine("Set up environment for an experiment");

                environment.ConductExperiment();

                List<ExperimentState> results = environment.GetPossibleExperimentResults();

                Console.WriteLine("Experiment results successfully calculated");

                WriteResultsToFile(results, outputPath);

                Console.WriteLine("Results succesfully written to file");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType() + ". " + ex.Message);
            }

            Console.WriteLine("Press any key to close the program");
            Console.ReadKey();
        }
        static string ReadContentsFromFile(string path)
        {
            StreamReader? sourceFile = new StreamReader(path);
            if (sourceFile is null)
                throw new ArgumentNullException("Input file not found");

            string sourceFileContents = sourceFile.ReadToEnd().Trim();

            sourceFile.Close();

            return sourceFileContents;
        }
        static ParticleEnvironment GetParticleEnvironmentFromString(string contents)
        {
            string[] lines = contents.Split("\r\n");

            int numberOfParticleTypes = GetNumberOfParticleTypes(lines);

            CheckForProperNumberOfLines(lines, numberOfParticleTypes);

            int[] numberOfParticles = GetNumberOfParticles(lines, numberOfParticleTypes);

            int[,] destructionMatrix = GetDestructionMatrix(lines, numberOfParticleTypes);

            return new ParticleEnvironment(new ExperimentState(numberOfParticles), destructionMatrix);
        }
        static int GetNumberOfParticleTypes(string[] lines)
        {
            bool convertedSuccesfully = true;

            int numberOfParticleTypes;
            convertedSuccesfully = int.TryParse(lines[0], out numberOfParticleTypes);
            if (!convertedSuccesfully)
                throw new ArgumentException("Can't get a number of particles from \"" + lines[0] + "\"");

            if (numberOfParticleTypes < 1 || numberOfParticleTypes > 4)
                throw new ArgumentException("The number of particle types must be between 1 and 4");

            return numberOfParticleTypes;
        }
        static void CheckForProperNumberOfLines(string[] lines, int numberOfParticleTypes)
        {
            int properNumberOfLines = 1 + 1 + numberOfParticleTypes;
            if (lines.Length != properNumberOfLines)
                throw new ArgumentException("Wrong number of input lines. Expected " + properNumberOfLines +
                    ", got " + lines.Length);
        }
        static int[] GetNumberOfParticles(string[] lines, int numberOfParticleTypes)
        {
            string[] numberOfParticlesStrings = lines[1].Split(" ");
            if (numberOfParticlesStrings.Length != numberOfParticleTypes)
                throw new ArgumentException("List of numbers of particles do not match the number of particle types");

            int[] numberOfParticles = new int[numberOfParticleTypes];
            for (int i = 0; i < numberOfParticleTypes; i++)
            {
                bool convertedSuccesfully = int.TryParse(numberOfParticlesStrings[i], out numberOfParticles[i]);
                if (!convertedSuccesfully)
                    throw new ArgumentException("Could not process list of numbers of particles: \"" + lines[1] + "\"");
            }
            return numberOfParticles;
        }
        static int[,] GetDestructionMatrix(string[] lines, int numberOfParticleTypes)
        {
            int[,] destructionMatrix = new int[numberOfParticleTypes, numberOfParticleTypes];
            for (int i = 0; i < numberOfParticleTypes; i++)
            {
                string currentLine = lines[2 + i];
                string[] lineNumbers = currentLine.Split(" ");
                if (lineNumbers.Length != numberOfParticleTypes)
                    throw new ArgumentException("Wrong quantity of numbers in line: \"" + currentLine + "\". Expected " + numberOfParticleTypes);

                for (int j = 0; j < numberOfParticleTypes; j++)
                {
                    bool convertedSuccesfully = int.TryParse(lineNumbers[j], out destructionMatrix[i, j]);
                    if (!convertedSuccesfully)
                        throw new ArgumentException("Could not convert number \"" + lineNumbers[j] +
                            "\" in line \"" + currentLine + "\"");
                }
            }
            return destructionMatrix;
        }

        static void WriteResultsToFile(List<ExperimentState> experimentResults, string path)
        {
            StreamWriter? resultsFile = new StreamWriter(path);

            if (resultsFile is null)
                throw new ArgumentNullException("Output file not found");

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("" + experimentResults.Count);
            foreach (var result in experimentResults)
            {
                foreach (var particleNumber in result.NumberOfParticles)
                    builder.Append(particleNumber + " ");
                builder.AppendLine("");
            }

            resultsFile.Write(builder.ToString());

            resultsFile.Close();
        }

        class ParticleEnvironment
        {
            public ExperimentState InitialState { get; private set; }
            public int[,] DestructionMatrix { get; private set; }

            public ParticleEnvironment(ExperimentState initialState, int[,] destructionMatrix)
            {
                InitialState = initialState;
                DestructionMatrix = destructionMatrix;
            }

            public void ConductExperiment()
            {
                InitialState.GenerateNextSteps(DestructionMatrix);
            }
            public List<ExperimentState> GetPossibleExperimentResults()
            {
                return GetUniqueStates(InitialState.GetFinishedExperimentStates());
            }
            private List<ExperimentState> GetUniqueStates(List<ExperimentState> states)
            {
                List<ExperimentState> uniqueStates = new List<ExperimentState>();
                foreach (var state in states)
                    if (!HasIdenticalLeafStateIn(uniqueStates, state))
                        uniqueStates.Add(state);
                return uniqueStates;
            }
            private bool HasIdenticalLeafStateIn(List<ExperimentState> states, ExperimentState currentState)
            {
                foreach (var state in states)
                    if (currentState.NumberOfParticles.SequenceEqual(state.NumberOfParticles))
                        return true;
                return false;
            }
        }
        class ExperimentState
        {
            public int[] NumberOfParticles { get; private set; }
            public List<ExperimentState> PossibleNextStates { get; private set; }

            public ExperimentState(int[] numberOfParticles)
            {
                NumberOfParticles = numberOfParticles;
                PossibleNextStates = new List<ExperimentState>();
            }
            public void GenerateNextSteps(int[,] destructionMatrix)
            {
                for (int i = 0; i < NumberOfParticles.Length; i++)
                    for (int j = 0; j < NumberOfParticles.Length; j++)
                    {
                        bool destroys = destructionMatrix[i, j] != 0;
                        bool destroysItself = destroys && i == j && NumberOfParticles[j] > 1;
                        bool destroysOtherParticle = destroys && i != j && NumberOfParticles[i] > 0 && NumberOfParticles[j] > 0;
                        if (destroysItself || destroysOtherParticle)
                        {
                            int[] newNumberOfParticles = new int[NumberOfParticles.Length];
                            NumberOfParticles.CopyTo(newNumberOfParticles, 0);
                            newNumberOfParticles[j]--;

                            PossibleNextStates.Add(new ExperimentState(newNumberOfParticles));
                        }
                    }

                foreach (var nextState in PossibleNextStates)
                    nextState.GenerateNextSteps(destructionMatrix);
            }
            public List<ExperimentState> GetFinishedExperimentStates()
            {
                List<ExperimentState> finishedExperimentStates = new List<ExperimentState>();
                if (PossibleNextStates.Count == 0)
                    finishedExperimentStates.Add(this);
                else
                    foreach (var experimentState in PossibleNextStates)
                        finishedExperimentStates.AddRange(experimentState.GetFinishedExperimentStates());

                return finishedExperimentStates;
            }
        }
    }
}
