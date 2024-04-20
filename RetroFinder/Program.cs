using System;
using System.IO;
using System.Linq;

namespace RetroFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "test_sequence.fa";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName);

            RetroFinder retroFinder = new RetroFinder();
            retroFinder.Analyze(path);
        }
    }
}
