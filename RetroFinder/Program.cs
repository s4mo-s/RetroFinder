using System;

namespace RetroFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            RetroFinder retroFinder = new RetroFinder();

            try
            {
                ArgumentParser.ParseArguments(args);
                retroFinder.Analyze(ArgumentParser.Path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
    }
}
