using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetroFinder.Output
{
    public class JsonOutputGenerator : ISerializer
    {
        public class TupleConverter : JsonConverter<(int, int)>
        {
            public override (int, int) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, (int, int) value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteNumber("start", value.Item1);
                writer.WriteNumber("end", value.Item2);
                writer.WriteEndObject();
            }
        }
        public void SerializeAnalysisResult(SequenceAnalysis sequenceAnalysis)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter(), new TupleConverter() }
                };
                string jsonString = JsonSerializer.Serialize(sequenceAnalysis.Transposons, options);

                string directoryPath = "out";
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), directoryPath, $"{sequenceAnalysis.Sequence.Id}.json");
                File.WriteAllText(outputPath, jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception($"creation of json failed({ex.Message})");
            }
        }
    }
}