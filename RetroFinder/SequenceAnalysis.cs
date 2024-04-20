using RetroFinder.Domains;
using RetroFinder.Models;
using RetroFinder.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RetroFinder
{
    public class SequenceAnalysis
    {
        public FastaSequence Sequence { get; set; }

        public IEnumerable<Transposon> Transposons { get; set; }

        public void Analyze()
        {
            Console.WriteLine($"Analyzing sequence {Sequence.Id}");
            var LtrFinder = new LTRFinder { Sequence = Sequence };
            Transposons = LtrFinder.IdentifyElements();
            List<Transposon> updatedTransposons = new List<Transposon>();
            foreach (Transposon transposon in Transposons)
            {
                string sequenceWithoutLTR = Sequence.Sequence.Substring(transposon.Features.First().Location.end, Sequence.Sequence.Length - transposon.Features.Last().Location.start);
                var domainFinder = new DomainFinder { Sequence = sequenceWithoutLTR };
                IEnumerable<Feature> features = domainFinder.IdentifyDomains();

                foreach (Feature feature in features)
                    feature.Location = (feature.Location.start + transposon.Location.start, feature.Location.end + transposon.Location.start);

                List<Feature> featuresUpdated =
                [
                    transposon.Features.First(),
                    .. features,
                    transposon.Features.Last(),
                ];
                Transposon updatedTransposon = new Transposon
                {
                    Location = (transposon.Location.start, transposon.Location.end),
                    Features = new List<Feature>()
                };

                updatedTransposon.Features = featuresUpdated;
                updatedTransposons.Add(updatedTransposon);
                //Console.WriteLine($"Transposon:  {transposon.Location.start} - {transposon.Location.end}");
                //Console.WriteLine($"{transposon.Features.First().Type}:  {transposon.Features.First().Location.start} - {transposon.Features.First().Location.end}");
                //Console.WriteLine($"{transposon.Features.Last().Type}: {transposon.Features.Last().Location.start} - {transposon.Features.Last().Location.end}");
            }
            JsonOutputGenerator.GenerateJsonFiles(Sequence, updatedTransposons);
        }
    }
}
