using System;
using System.IO;
using System.Xml;

namespace RetroFinder.Output
{
    public class XmlOutputGenerator : ISerializer
    {
        public void SerializeAnalysisResult(SequenceAnalysis analysis)
        {
            try
            {
                string directoryPath = "out";
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), directoryPath, $"{analysis.Sequence.Id}.xml");

                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

                using (XmlWriter writer = XmlWriter.Create(outputPath, settings))
                {
                    // Root
                    writer.WriteStartElement("Transposons");

                    foreach (var transposon in analysis.Transposons)
                    {
                        // Transposon
                        writer.WriteStartElement("Transposon");

                        writer.WriteStartElement("Location");
                        writer.WriteAttributeString("Start", transposon.Location.start.ToString());
                        writer.WriteAttributeString("End", transposon.Location.end.ToString());
                        writer.WriteEndElement();

                        // Features
                        writer.WriteStartElement("Features");
                        foreach (var feature in transposon.Features)
                        {
                            // Feature
                            writer.WriteStartElement("Feature");

                            writer.WriteElementString("Type", feature.Type.ToString());

                            writer.WriteStartElement("Location");
                            writer.WriteAttributeString("Start", feature.Location.start.ToString());
                            writer.WriteAttributeString("End", feature.Location.end.ToString());
                            writer.WriteEndElement();

                            writer.WriteEndElement(); // End Feature
                        }
                        writer.WriteEndElement(); // End Features

                        writer.WriteEndElement(); // End Transposon
                    }

                    writer.WriteEndElement(); // End Root
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"creation of xml failed({ex.Message})");
            }
        }
    }
}