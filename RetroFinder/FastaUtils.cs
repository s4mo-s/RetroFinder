using RetroFinder.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RetroFinder
{
    public class FastaUtils
    {
        public static bool Validate(string path)
        {
            var lines = File.ReadLines(path);
            HashSet<string> ids = new HashSet<string>();
            foreach (var line in lines)
            {
                if (line.StartsWith(">"))
                {
                    if (string.IsNullOrEmpty(line.Substring(1).Trim()))
                        return false;

                    if (ids.Contains(line))
                        return false;

                    ids.Add(line);
                }
                else
                {
                    if (line.Any(c => !"ACGTN".Contains(c)))
                        return false;
                }
            }

            return ids.Any();
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
