using RetroFinder.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RetroFinder.Output
{
    public class JsonOutputGenerator
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
        public static void GenerateJsonFile(FastaSequence sequence, IEnumerable<Transposon> transposons)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(), new TupleConverter() }
            };

            try
            {
                string jsonString = JsonSerializer.Serialize(transposons, options);

                string directoryPath = "out";
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), directoryPath, $"{sequence.Id}.json");
                File.WriteAllText(outputPath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Creation of json failed: {ex.Message}");
            }
        }
    }
}