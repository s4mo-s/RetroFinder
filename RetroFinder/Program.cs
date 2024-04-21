using System;

namespace RetroFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ArgumentParser.ParseArguments(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            RetroFinder retroFinder = new RetroFinder();
            retroFinder.Analyze(ArgumentParser.path);
        }
    }
}
