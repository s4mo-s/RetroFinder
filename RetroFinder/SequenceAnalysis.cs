using RetroFinder.Models;
using System;
using System.Collections.Generic;

namespace RetroFinder
{
    public class SequenceAnalysis
    {
        public FastaSequence Sequence { get; set; }

        public IEnumerable<Transposon> Transposons { get; set; }

        public void Analyze()
        {
            throw new NotImplementedException();
        }
    }
}
