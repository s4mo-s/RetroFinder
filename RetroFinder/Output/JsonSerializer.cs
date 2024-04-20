using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using RetroFinder.Models;

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
        public static void GenerateJsonFiles(FastaSequence sequence, IEnumerable<Transposon> transposons)
        {
            // Vytvorte cestu k výstupnému súboru s názvom rovnakým ako ID sekvencie
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "out", $"{sequence.Id}.json");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(), new TupleConverter() }
            };

            // Serializujte len nájdené transpozóny s properties, ktoré máme v kostre

            // Pretty-printnutý JSON súbor
            string jsonString = JsonSerializer.Serialize(transposons, options);

            // Zapište JSON do súboru
            File.WriteAllText(outputPath, jsonString);
        }
    }
}