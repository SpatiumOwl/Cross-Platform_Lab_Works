﻿using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;

namespace Lab_04_Prog
{
    public class Program
    {
        delegate void LabLaunchCommand(string inputPath, string outputPath);

        public static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "lab04",
                Description = "Lab04 - Lab01-03 launcher",
            };
            app.HelpOption(inherited: true);
            app.Command("version", verCmd =>
            {
                verCmd.Description = "Display the version of the program";
                verCmd.OnExecute(() =>
                {
                    string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    Console.WriteLine("Author: Ivan Kholiavkin, Version: " + version);
                });
            });
            app.Command("run", runCmd =>
            {
                runCmd.Description = "Run the lab work (lab1, lab2, lab3)";
                runCmd.OnExecute(() =>
                {
                    runCmd.ShowHelp();
                    return 1;
                });
                runCmd.Command("lab1", labCmd => 
                {
                    var inputArg = labCmd.Option("-i|--input", "Input path", CommandOptionType.SingleOrNoValue);
                    inputArg.DefaultValue = "";

                    var outputArg = labCmd.Option("-o|--output", "Output path", CommandOptionType.SingleOrNoValue);
                    outputArg.DefaultValue = "";
                    labCmd.OnExecute(() =>
                    {
                        Console.WriteLine(inputArg.Value() + " " + outputArg.Value());
                        LaunchLab(Lab_04_Lib.Lab01.Launch, new KeyValuePair<string, string>(inputArg.Value(), outputArg.Value()));
                    });
                });
                runCmd.Command("lab2", labCmd =>
                {
                    var inputArg = labCmd.Option("-i|--input", "Input path", CommandOptionType.SingleOrNoValue);
                    inputArg.DefaultValue = "";

                    var outputArg = labCmd.Option("-o|--output", "Output path", CommandOptionType.SingleOrNoValue);
                    outputArg.DefaultValue = "";
                    labCmd.OnExecute(() =>
                    {
                        Console.WriteLine(inputArg.Value() + " " + outputArg.Value());
                        LaunchLab(Lab_04_Lib.Lab02.Launch, new KeyValuePair<string, string>(inputArg.Value(), outputArg.Value()));
                    });
                });
                runCmd.Command("lab3", labCmd =>
                {
                    var inputArg = labCmd.Option("-i|--input", "Input path", CommandOptionType.SingleOrNoValue);
                    inputArg.DefaultValue = "";

                    var outputArg = labCmd.Option("-o|--output", "Output path", CommandOptionType.SingleOrNoValue);
                    outputArg.DefaultValue = "";
                    labCmd.OnExecute(() =>
                    {
                        Console.WriteLine(inputArg.Value() + " " + outputArg.Value());
                        LaunchLab(Lab_04_Lib.Lab03.Launch, new KeyValuePair<string, string>(inputArg.Value(), outputArg.Value()));
                    });
                });
            });
            app.Command("set-path", setCmd =>
            {
                setCmd.Description = "Set path to the input and output files folder";
                var labPath = setCmd.Option("-p|--path", "Path to the folder with input and output files", CommandOptionType.SingleValue).IsRequired();
                setCmd.OnExecute(() =>
                {
                    Environment.SetEnvironmentVariable("LAB_PATH", labPath.Value(), EnvironmentVariableTarget.User);
                });
            });

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 1;
            });

            return app.Execute(args);
        }
        static void LaunchLab(LabLaunchCommand launchCommand, KeyValuePair<string, string> ioOptions)
        {
            string inputOption = ioOptions.Key;
            string outputOption = ioOptions.Value;
            
            string? labPath = Environment.GetEnvironmentVariable("LAB_PATH", EnvironmentVariableTarget.User);
            string workingDirectory = Environment.CurrentDirectory;

            Console.WriteLine(labPath);

            if (inputOption != "" && outputOption != "")
                launchCommand(inputOption, outputOption);
            else 
            {
                var labIOFiles = GetInputAndOutputFromFolder(labPath);
                if (labIOFiles.Count > 0)
                    foreach (KeyValuePair<string, string> pair in labIOFiles)
                        launchCommand(pair.Key, pair.Value);
                else
                {
                    var homeIOFiles = GetInputAndOutputFromFolder(workingDirectory);
                    if (homeIOFiles.Count > 0)
                        foreach (KeyValuePair<string, string> pair in homeIOFiles)
                            launchCommand(pair.Key, pair.Value);
                    else
                        Console.WriteLine("ERROR. IO files not found");
                }
            }
        }
        static Dictionary<string, string> GetInputAndOutputFromFolder(string? dirPath)
        {
            var result = new Dictionary<string, string>();

            if (!Directory.Exists(dirPath))
                return result;

            string[] files = Directory.GetFiles(dirPath);
            foreach (var file in files)
                if (file.EndsWith(".txt"))
                {
                    string inputFile = file;
                    string outputFile = file.Substring(0, file.Length - 4) + "_output.txt";
                    result.Add(inputFile, outputFile);
                }

            return result;
        }
    }
}