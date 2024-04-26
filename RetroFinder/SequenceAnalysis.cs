using RetroFinder.Domains;
using RetroFinder.Models;
using RetroFinder.Output;
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
            Console.WriteLine($"Analyzing {Sequence.Id}");

            LTRFinder LtrFinder = new LTRFinder { Sequence = Sequence };
            Transposons = LtrFinder.IdentifyElements();

            List<Transposon> updatedTransposons = [];

            foreach (Transposon transposon in Transposons)
            {
                int lengthBetweenLTRs = transposon.Features.Last().Location.start - transposon.Features.First().Location.end;
                string sequenceWithoutLTR = Sequence.Sequence.Substring(transposon.Features.First().Location.end + 1, lengthBetweenLTRs - 1);
                DomainFinder domainFinder = new DomainFinder { Sequence = sequenceWithoutLTR };
                IEnumerable<Feature> pickedFeatures = domainFinder.IdentifyDomains();

                foreach (Feature feature in pickedFeatures)
                    feature.Location = (transposon.Features.First().Location.end + feature.Location.start, transposon.Features.First().Location.end + feature.Location.end);

                List<Feature> featuresUpdated = [transposon.Features.First(), .. pickedFeatures, transposon.Features.Last()];
                Transposon updatedTransposon = new Transposon { Location = (transposon.Location.start, transposon.Location.end), Features = featuresUpdated };
                updatedTransposons.Add(updatedTransposon);

                //Console.WriteLine($"Transposon:  {transposon.Location.start} - {transposon.Location.end}");
                //Console.WriteLine($"{transposon.Features.First().Type}:  {transposon.Features.First().Location.start} - {transposon.Features.First().Location.end}");
                //Console.WriteLine($"{transposon.Features.Last().Type}: {transposon.Features.Last().Location.start} - {transposon.Features.Last().Location.end}");
            }

            if (updatedTransposons.Count == 0)
                throw new Exception("transposons not found");

            Transposons = updatedTransposons;

            JsonOutputGenerator json = new JsonOutputGenerator();
            XmlOutputGenerator xml = new XmlOutputGenerator();

            if (ArgumentParser.Json)
                json.SerializeAnalysisResult(this);
            if (ArgumentParser.Xml)
                xml.SerializeAnalysisResult(this);

            Console.WriteLine($"{Sequence.Id}: Done");
        }
    }
}
