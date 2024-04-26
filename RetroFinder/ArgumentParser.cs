using System;
using System.Linq;

namespace RetroFinder
{
    public class ArgumentParser
    {
        public static string Path { get; set; }
        public static bool Json { get; set; } = false;
        public static bool Xml { get; set; } = false;

        public static void ParseArguments(string[] args)
        {
            if (args.Length < 2 || args.Contains("-help"))
                throw new Exception("Usage: dotnet run -path {filePath} -json -xml");

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-path")
                    Path = args[i + 1];

                if (args[i] == "-json")
                    Json = true;

                if (args[i] == "-xml")
                    Xml = true;
            }

            if (Path == null)
                throw new Exception("Path was not specified.\nUsage: dotnet run -path {filePath} -json -xml");
            if (!Json && !Xml)
                throw new Exception("Output was not specified.\nUsage: dotnet run -path {filePath} -json -xml");
        }
    }
}
