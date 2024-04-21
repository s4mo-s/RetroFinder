using RetroFinder.Models;
using System.Collections.Generic;
using System.Linq;

namespace RetroFinder.Domains
{
    public class DomainPicker
    {
        public List<(Feature, int)> identifiedDomains { get; set; }
        public IEnumerable<Feature> PickDomains()
        {
            List<Feature> selectedDomains = new List<Feature>();

            // Define the typical domain order
            FeatureType[] typicalOrder = { FeatureType.GAG, FeatureType.PROT, FeatureType.INT, FeatureType.RT, FeatureType.RH };

            foreach (var domainType in typicalOrder)
            {
                // Find the best match for each domain type
                var bestMatch = identifiedDomains
                    .Where(d => d.Item1.Type == domainType)
                    .OrderByDescending(d => d.Item2) // Order by alignment score (higher is better)
                    .FirstOrDefault();

                // Add the best match to selected domains if it does not overlap with existing ones
                if (bestMatch.Item1 != null && !selectedDomains.Any(d => d.Location.end > bestMatch.Item1.Location.start))
                {
                    selectedDomains.Add(bestMatch.Item1);
                }
            }

            return selectedDomains;
        }
    }
}
