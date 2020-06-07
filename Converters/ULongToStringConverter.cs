using System;
using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Searchfight.Converters
{
    /// <summary>
    /// Converts UInt64 value to or from JSON
    /// </summary>
    public class ULongToStringConverter : JsonConverter<UInt64>
    {
        /// <summary>
        /// Reads and converts the JSON to UInt64
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <param name="type">The type to convert</param>
        /// <param name="options">An object that specifies serialization options to use</param>
        /// <returns></returns>
        public override UInt64 Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out UInt64 number, out int bytesConsumed) && span.Length == bytesConsumed)
                {
                    return number;
                }

                if (UInt64.TryParse(reader.GetString(), out number))
                {
                    return number;
                }
            }

            return reader.GetUInt64();
        }

        /// <summary>
        ///  Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        /// <param name="value">The value to convert to JSON</param>
        /// <param name="options">An object that specifies serialization options to use</param>
        public override void Write(Utf8JsonWriter writer, UInt64 value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}