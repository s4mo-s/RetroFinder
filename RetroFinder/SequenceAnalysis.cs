using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroFinder
{
    public class SequenceAnalysis
    {
        public FastaSequence Sequence { get; set; }

        public IEnumerable<Transposon> Transposons { get; set; }

        public void Analyze()
        {
            Console.WriteLine($"Analyzing sequence {Sequence.Id}");
            var LTRFinder = new LTRFinder { Sequence = Sequence };
            Transposons = LTRFinder.IdentifyElements();
            /* foreach (Transposon transposon in Transposons)
            {
                Console.WriteLine($"Transposon:  {transposon.Location.start} - {transposon.Location.end}");
                Console.WriteLine($"{transposon.Features.First().Type}:  {transposon.Features.First().Location.start} - {transposon.Features.First().Location.end}");
                Console.WriteLine($"{transposon.Features.Last().Type}: {transposon.Features.Last().Location.start} - {transposon.Features.Last().Location.end}");
            } */


        }
    }
}
