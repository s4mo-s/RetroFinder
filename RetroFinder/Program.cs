using System;
using System.IO;

namespace RetroFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "test_sequence.fa";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName);

            RetroFinder retroFinder = new RetroFinder();
            retroFinder.Analyze(filePath);
        }
    }
}
