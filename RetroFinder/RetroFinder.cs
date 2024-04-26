using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RetroFinder.Models;

namespace RetroFinder
{
    public class RetroFinder
    {
        public IEnumerable<FastaSequence> sequences;

        public void Analyze(string path)
        {
            if (FastaUtils.Validate(path))
                sequences = FastaUtils.Parse(path);

            SemaphoreSlim semaphore = new SemaphoreSlim(5, 5);
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Analysis of {sequence.Id} failed: {ex.Message}");
                    return;
                }
                finally
                {
                    semaphore.Release();
                }
            });
        }
    }
}
