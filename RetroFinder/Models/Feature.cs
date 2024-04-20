namespace RetroFinder.Models
{
    public class Feature
    {
        public FeatureType Type { get; set; }

        public (int start, int end) Location { get; set; }
    }
}
