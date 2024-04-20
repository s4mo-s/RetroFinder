using RetroFinder.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RetroFinder
{
    public class LTRFinder
    {
        public FastaSequence Sequence { get; set; }

        public IEnumerable<Transposon> IdentifyElements()
        {
            List<Transposon> transposons = new List<Transposon>();
            List<(int start, int end, string sequence)> ltrs = FindLTRs(Sequence.Sequence);

            if (ltrs.Count < 2)
                yield break;

            // Iterate through each pair of LTRs
            for (int i = 0; i < ltrs.Count - 1; i++)
            {
                for (int j = i + 1; j < ltrs.Count; j++)
                {
                    if (AreIdenticalLTRs(ltrs[i].sequence, ltrs[j].sequence))
                    {
                        Transposon transposon = new Transposon
                        {
                            Location = (ltrs[i].start, ltrs[j].end),
                            Features = new List<Feature>()
                        };

                        transposon.Features = new List<Feature>
                        {
                            new Feature
                            {
                                Type = FeatureType.LTRLeft,
                                Location = (ltrs[i].start, ltrs[i].end),
                                Annotation = "Left LTR"
                            },
                            new Feature
                            {
                                Type = FeatureType.LTRRight,
                                Location = (ltrs[j].start, ltrs[j].end),
                                Annotation = "Right LTR"
                            }
                        };
                        transposons.Add(transposon);
                    }
                }
            }
            foreach (var transposon in transposons)
                yield return transposon;
        }

        private static List<(int start, int end, string sequence)> FindLTRs(string sequence)
        {
            List<(int start, int end, string sequence)> ltrs = new List<(int start, int end, string sequence)>();

            string pattern = @"([ACGT]{100,300}).{1000,3500}(\1)";
            MatchCollection matches = Regex.Matches(sequence, pattern);

            foreach (Match match in matches)
            {
                int leftLtrStart = match.Index;
                int leftLtrEnd = leftLtrStart + match.Groups[1].Length - 1;
                string ltrSequence = match.Groups[1].Value;
                ltrs.Add((leftLtrStart, leftLtrEnd, ltrSequence));

                int rightLtrStart = match.Groups[2].Index;
                int rightLtrEnd = rightLtrStart + match.Groups[2].Length - 1;
                string rightLtrSequence = match.Groups[2].Value;
                ltrs.Add((rightLtrStart, rightLtrEnd, rightLtrSequence));
            }
            return ltrs;
        }

        private static bool AreIdenticalLTRs(string ltr1, string ltr2) => ltr1.Equals(ltr2);
    }
}
