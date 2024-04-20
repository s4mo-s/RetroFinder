using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RetroFinder.Domains
{
    public class DomainFinder
    {
        public class SmithWatermanResult
        {
            public int Score { get; set; }
            public List<(int, int)> AlignmentCoordinates { get; set; }
        }

        public IEnumerable<FastaSequence> DomainSequences;
        public string Sequence { get; set; }

        public IEnumerable<Feature> IdentifyDomains()
        {
            string fileName = "protein_domains.fa";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName);

            if (FastaUtils.Validate(filePath))
                DomainSequences = FastaUtils.Parse(filePath);

            List<Feature> identifiedDomains = new List<Feature>();
            List<(Feature, int)> identifiedDomainScores = new List<(Feature, int)>();

            foreach (var domain in DomainSequences)
            {
                SmithWatermanResult result = Align(Sequence, domain.Sequence, gapPenalty: -2);

                // Check if the alignment score meets a certain threshold
                if (result.Score >= 100) // Adjust the threshold as needed
                {
                    // Add the identified domain to the list of features
                    FeatureType type = GetFeatureType(domain.Id);

                    Console.WriteLine(type + ": " + result.Score);
                    int start = result.AlignmentCoordinates.First().Item1;
                    int alignmentLength = result.AlignmentCoordinates.Count;
                    int end = start + alignmentLength - 1;

                    Feature identifiedDomain = new Feature
                    {
                        Type = type,
                        Location = (start, end),
                    };

                    identifiedDomains.Add(identifiedDomain);
                    identifiedDomainScores.Add((identifiedDomain, result.Score));
                }
            }

            DomainPicker domainPicker = new DomainPicker { identifiedDomains = identifiedDomainScores };
            Console.WriteLine("");
            return domainPicker.PickDomains();
        }

        private SmithWatermanResult Align(string seq1, string seq2, int gapPenalty)
        {
            /* int[,] scoreMatrix = CalculateScoreMatrix(seq1, seq2);
            var maxCell = FindMaxCell(scoreMatrix);
            var alignmentCoordinates = Traceback(scoreMatrix, maxCell);

            return new SmithWatermanResult
            {
                Score = scoreMatrix[maxCell.Item1, maxCell.Item2],
                AlignmentCoordinates = alignmentCoordinates
            }; */

            // Initialize score matrix
            int[,] scoreMatrix = new int[seq1.Length + 1, seq2.Length + 1];
            int[,] traceback = new int[seq1.Length + 1, seq2.Length + 1];
            List<(int, int)> alignmentCoordinates = new List<(int, int)>();

            // Initialize first row and first column to 0
            for (int i = 0; i <= seq1.Length; i++)
                scoreMatrix[i, 0] = 0;
            for (int j = 0; j <= seq2.Length; j++)
                scoreMatrix[0, j] = 0;

            int maxScore = 0;
            (int, int) maxScorePosition = (0, 0);

            // Fill in the score matrix using the recurrence relation
            for (int i = 1; i <= seq1.Length; i++)
            {
                for (int j = 1; j <= seq2.Length; j++)
                {
                    int matchScore = seq1[i - 1] == seq2[j - 1] ? 3 : -3;
                    int diagonalScore = scoreMatrix[i - 1, j - 1] + matchScore;
                    int leftScore = scoreMatrix[i, j - 1] - 2;
                    int upScore = scoreMatrix[i - 1, j] - 2;

                    int score = Math.Max(0, Math.Max(diagonalScore, Math.Max(leftScore, upScore)));
                    scoreMatrix[i, j] = score;

                    if (score > maxScore)
                    {
                        maxScore = score;
                        maxScorePosition = (i, j);
                    }

                    if (score == diagonalScore)
                    {
                        traceback[i, j] = 0; // Diagonal
                    }
                    else if (score == upScore)
                    {
                        traceback[i, j] = 1; // Up
                    }
                    else if (score == leftScore)
                    {
                        traceback[i, j] = -1; // Left
                    }

                }
            }

            // Traceback
            int row = maxScorePosition.Item1;
            int col = maxScorePosition.Item2;

            while (scoreMatrix[row, col] != 0)
            {
                alignmentCoordinates.Add((row, col));

                int predecessor = traceback[row, col];
                if (predecessor == 0)
                {
                    row--;
                    col--;
                }
                else if (predecessor == 1)
                {
                    row--;
                }
                else if (predecessor == -1)
                {
                    col--;
                }
            }

            alignmentCoordinates.Reverse();

            // Filter alignments shorter than half of the database sequence length
            if (alignmentCoordinates.Count < seq2.Length / 2)
            {
                return new SmithWatermanResult
                {
                    Score = 0,
                    AlignmentCoordinates = new List<(int, int)>()
                };
            }

            return new SmithWatermanResult
            {
                Score = maxScore,
                AlignmentCoordinates = alignmentCoordinates
            };

        }

        private int[,] CalculateScoreMatrix(string seq1, string seq2)
        {
            // Initialize score matrix
            int[,] scoreMatrix = new int[seq1.Length + 1, seq2.Length + 1];

            // Initialize first row and first column to 0
            for (int i = 0; i <= seq1.Length; i++)
                scoreMatrix[i, 0] = 0;
            for (int j = 0; j <= seq2.Length; j++)
                scoreMatrix[0, j] = 0;

            // Fill in the score matrix using the recurrence relation
            for (int i = 1; i <= seq1.Length; i++)
            {
                for (int j = 1; j <= seq2.Length; j++)
                {
                    int matchScore = seq1[i - 1] == seq2[j - 1] ? 3 : -3;
                    int diagonalScore = scoreMatrix[i - 1, j - 1] + matchScore;
                    int leftScore = scoreMatrix[i, j - 1] - 2; // Gap penalty
                    int upScore = scoreMatrix[i - 1, j] - 2; // Gap penalty

                    scoreMatrix[i, j] = Math.Max(0, Math.Max(diagonalScore, Math.Max(leftScore, upScore)));
                }
            }
            return scoreMatrix;
        }

        private (int, int) FindMaxCell(int[,] scoreMatrix)
        {
            int maxScore = int.MinValue;
            (int, int) maxCell = (0, 0);

            for (int i = 0; i < scoreMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < scoreMatrix.GetLength(1); j++)
                {
                    if (scoreMatrix[i, j] > maxScore)
                    {
                        maxScore = scoreMatrix[i, j];
                        maxCell = (i, j);
                    }
                }
            }
            return maxCell;
        }

        private FeatureType GetFeatureType(string id)
        {
            return id switch
            {
                var s when s.Contains("PROT") => FeatureType.PROT,
                var s when s.Contains("GAG") => FeatureType.GAG,
                var s when s.Contains("INT") => FeatureType.INT,
                var s when s.Contains("RT") => FeatureType.RT,
                var s when s.Contains("RH") => FeatureType.RH,
                _ => throw new Exception("Wrong featureType.")
            };
        }

    }
}
