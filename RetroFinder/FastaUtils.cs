using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RetroFinder
{
    public class FastaUtils
    {
        public static bool Validate(string path)
        {
            HashSet<string> ids = new HashSet<string>();
            var lines = File.ReadLines(path);
            if (lines.Count() < 2)
                throw new Exception("File validation: file must contain at least one ID with sequence");
            if (lines.Count() % 2 == 1)
                throw new Exception("File validation: missing pair of ID with sequence");

            foreach (var line in lines)
            {
                if (line.StartsWith(">"))
                {
                    string id = line.Substring(1).Trim();

                    if (string.IsNullOrEmpty(id))
                        throw new Exception("File validation: ID is empty");

                    if (ids.Contains(id))
                        throw new Exception($"File validation: {id} is not unique");

                    ids.Add(id);
                }
                else
                {
                    if (string.IsNullOrEmpty(line.Trim()))
                        throw new Exception($"File validation: sequence of {ids.Last()} is empty");

                    if (line.Any(c => !"ACGTN".Contains(c)))
                        throw new Exception($"File validation: sequence of {ids.Last()} contains invalid characters");
                }
            }

            return ids.Count != 0;
        }

        public static IEnumerable<FastaSequence> Parse(string path)
        {
            List<FastaSequence> fastaSequences = new List<FastaSequence>();

            var lines = File.ReadLines(path);
            string currentId = null;
            string currentSequence = "";

            foreach (var line in lines)
            {
                if (line.StartsWith(">"))
                {
                    if (!string.IsNullOrEmpty(currentId))
                    {
                        fastaSequences.Add(new FastaSequence(currentId, currentSequence));
                        currentSequence = "";
                    }
                    currentId = line.Substring(1).Trim();
                }
                else
                    currentSequence += line.Trim();
            }

            if (!string.IsNullOrEmpty(currentId))
                fastaSequences.Add(new FastaSequence(currentId, currentSequence));

            return fastaSequences;
        }
    }
}
