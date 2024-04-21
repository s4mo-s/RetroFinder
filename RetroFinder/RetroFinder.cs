using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RetroFinder.Models;

namespace RetroFinder
{
    public class RetroFinder
    {
        private SemaphoreSlim semaphore;
        public IEnumerable<FastaSequence> sequences;

        public void Analyze(string path)
        {
            try
            {
                if (FastaUtils.Validate(path))
                    sequences = FastaUtils.Parse(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            var sequenceAnalyses = new SequenceAnalysis();
            semaphore = new SemaphoreSlim(5, 5);

            var tasks = new List<Task>();

            Parallel.ForEach(sequences, sequence =>
            {
                semaphore.Wait();
                try
                {
                    var analysis = new SequenceAnalysis
                    {
                        Sequence = sequence
                    };
                    analysis.Analyze();
                }
                finally
                {
                    semaphore.Release();
                }
            });
        }
    }
}
