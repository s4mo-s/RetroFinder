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
            FeatureType[] typicalOrder = { FeatureType.GAG, FeatureType.PROT, FeatureType.INT, FeatureType.RT, FeatureType.RH };
            int lastEnd = -1;

            foreach (var domainType in typicalOrder)
            {
                // Get all domains of the current type
                var domainsOfType = identifiedDomains.Where(x => x.Item1.Type == domainType).OrderByDescending(x => x.Item2).ToList();

                foreach (var domain in domainsOfType)
                {
                    if (domain.Item1.Location.start > lastEnd)
                    {
                        selectedDomains.Add(domain.Item1);
                        lastEnd = domain.Item1.Location.end;

                        //Console.WriteLine($"Selected domain: {domain.Item1.Type} ({domain.Item1.Location.start}, {domain.Item1.Location.end})");
                        break;
                    }
                    else
                    {
                        //Console.WriteLine($"Domain {domain.Item1.Type} overlaps and is not picked.");
                    }
                }
            }
            return selectedDomains;
        }
    }
}
