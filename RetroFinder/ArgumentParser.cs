using System;
using System.Linq;

namespace RetroFinder
{
    public class ArgumentParser
    {
        public static string path;
        public static bool json = false;
        public static bool xml = false;

        public static void ParseArguments(string[] args)
        {
            if (args.Length == 0 || args.Contains("-help"))
                throw new Exception("Usage: dotnet run -path {filePath} -json -xml");

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-path")
                    path = args[i + 1];

                if (args[i] == "-json")
                    json = true;

                if (args[i] == "-xml")
                    xml = true;
            }

            if (!json && !xml)
                throw new Exception("Output was not specified.\nUsage: dotnet run -path {filePath} -json -xml");
        }
    }
}
