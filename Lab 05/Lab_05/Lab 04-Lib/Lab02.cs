using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Linq;

namespace Lab_04_Lib
{
    public static class Lab02
    {
        public static void Launch(string inputPath, string outputPath)
        {
            Console.WriteLine("Processing the file");
            try
            {
                string fileContents = ReadContentsFromFile(inputPath);

                Console.WriteLine("Input file succesfully opened");

                List<Agent> agents = GetAgentsFromString(fileContents);
                int risk = GetMinimumRiskFromGroupingAgents(agents);

                Console.WriteLine("Succesfully finished the calculations");

                SaveResultToFile(outputPath, risk);

                Console.WriteLine("Succesfully saved the result into an output file");
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
        static List<Agent> GetAgentsFromString(string fileContents)
        {
            bool conversionSuccess;

            string[] lines = fileContents.Split("\r\n");
            if (lines.Length != 2)
                throw new ArgumentException("Incorrect file format (should be two lines)");

            string agentString = lines[1];

            string[] agentValues = agentString.Split(" ");

            int numberOfAgents;
            conversionSuccess = int.TryParse(lines[0], out numberOfAgents);

            if (!conversionSuccess)
                throw new FormatException("Number of agents was not in a correct format");

            if (numberOfAgents != agentValues.Length / 2)
                throw new ArgumentException("Given number of agents does not match the data");

            List<Agent> agents = new List<Agent>();

            for (int i = 0; i < agentValues.Length; i += 2)
            {
                int age = 0, risk = 0;
                conversionSuccess = conversionSuccess && int.TryParse(agentValues[i], out age);
                conversionSuccess = conversionSuccess && int.TryParse(agentValues[i + 1], out risk);

                if (!conversionSuccess)
                    throw new FormatException("Failed to convert agent data: " + agentValues[i] + " " + agentValues[i + 1]);

                agents.Add(new Agent(age, risk));
            }

            return agents;
        }
        static int GetMinimumRiskFromGroupingAgents(List<Agent> agents)
        {
            agents = SortAgents(agents);

            if (agents.Count < 2)
                throw new ArgumentException("Too little agents in a group");

            int[] minRiskBeforeIndex = new int[agents.Count];

            minRiskBeforeIndex[0] = agents[0].risk;

            if (agents.Count < 3)
                return minRiskBeforeIndex[0];

            minRiskBeforeIndex[1] = agents[1].risk;

            for (int i = 2; i < agents.Count; i++)
            {
                int variant1 = minRiskBeforeIndex[i - 2] + agents[i - 1].risk;
                int variant2 = minRiskBeforeIndex[i - 1] + agents[i - 1].risk;
                minRiskBeforeIndex[i] = (variant1 < variant2) ? variant1 : variant2;
            }

            return minRiskBeforeIndex[agents.Count - 1];
        }
        static List<Agent> SortAgents(List<Agent> agents)
        {
            return agents.OrderByDescending(agent => agent.age).ToList();
        }
        static void SaveResultToFile(string path, int result)
        {
            StreamWriter? resultsFile = new StreamWriter(path);

            if (resultsFile is null)
                throw new ArgumentNullException("Output file not found");

            resultsFile.Write(result);

            resultsFile.Close();
        }

        internal class Agent
        {
            public int age { get; set; }
            public int risk { get; set; }
            public Agent(int age, int risk)
            {
                this.age = age;
                this.risk = risk;
            }
        }
    }
}
