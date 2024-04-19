using System.Collections.Generic;

namespace RetroFinder.Models
{
    public class Transposon
    {
        public (int start, int end) Location { get; set; }

        public IEnumerable<Feature> Features { get; set; }
    }
}
