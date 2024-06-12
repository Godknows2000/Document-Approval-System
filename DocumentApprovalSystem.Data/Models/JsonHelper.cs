using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace DocumentApprovalSystem.Lib
{
    public static class JsonHelper
    {
        public static string JsonMime = "application/json";
        public static JsonSerializerOptions SerializerOptions => new(JsonSerializerDefaults.Web)
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
        };

        static JsonHelper()
        {
            SerializerOptions.Converters.Add(new StringConverter());
            SerializerOptions.Converters.Add(new IntConverter());
            SerializerOptions.Converters.Add(new DecimalConverter());
            SerializerOptions.Converters.Add(new DateConverter());
            SerializerOptions.Converters.Add(new DateOnlyConverter());
            SerializerOptions.Converters.Add(new TimeOnlyConverter());
        }

        class IntConverter : JsonConverter<int>
        {
            public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                try
                {
                    return reader.GetInt32();
                }
                catch
                {
                    return int.Parse(reader.GetString());
                }
            }

            public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }
        public class StringConverter : JsonConverter<object>
        {
            public override bool CanConvert(Type typeToConvert)
            {
                return typeof(string) == typeToConvert;
            }
            public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Number)
                {
                    return reader.TryGetInt64(out long l) ?
                        l.ToString() :
                        reader.GetDouble().ToString();
                }
                if (reader.TokenType == JsonTokenType.String)
                {
                    return reader.GetString();
                }
                using (JsonDocument document = JsonDocument.ParseValue(ref reader))
                {
                    return document.RootElement.Clone().ToString();
                }
            }

            public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        //class StringConverter : JsonConverter<string>
        //{
        //    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //    {
        //        if (reader.TokenType == JsonTokenType.Number)
        //        {
        //            var stringValue = reader.GetInt32();
        //            return stringValue.ToString();
        //        }
        //        else if (reader.TokenType == JsonTokenType.String)
        //        {
        //            return reader.GetString();
        //        }

        //        throw new System.Text.Json.JsonException();
        //    }

        //    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        //    {
        //        writer.WriteStringValue(value);
        //    }

        //}

        class DecimalConverter : JsonConverter<decimal>
        {
            public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                try
                {
                    return reader.GetDecimal();
                }
                catch
                {
                    return decimal.Parse(reader.GetString());
                }
            }

            public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value);
            }
        }
        class DateConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                try
                {
                    return reader.GetDateTime();
                }
                catch (Exception ex)
                {
                    var value = reader.GetString();
                    if (DateTime.TryParse(value, out DateTime date)) return date;
                    var match = Regex.Match(value, @"/Date\((-?\d+)\)/");
                    if (match.Success && long.TryParse(match.Groups[1].Value, out long ms))
                    {
                        return DateTime.UnixEpoch.AddMilliseconds(ms);
                    }
                    throw ex;
                }
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("O"));
            }
        }

        class DateOnlyConverter : JsonConverter<System.DateOnly>
        {
            private const string Format = "yyyy-MM-dd";

            public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateOnly.ParseExact(reader.GetString(), Format, CultureInfo.InvariantCulture);
            }

            public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
            }
        }

        class TimeOnlyConverter : JsonConverter<TimeOnly>
        {
            private readonly string serializationFormat;

            public TimeOnlyConverter() : this(null)
            {
            }

            public TimeOnlyConverter(string? serializationFormat)
            {
                this.serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
            }

            public override TimeOnly Read(ref Utf8JsonReader reader,
                                    Type typeToConvert, JsonSerializerOptions options)
            {
                var value = reader.GetString();
                return TimeOnly.Parse(value!);
            }
            public override void Write(Utf8JsonWriter writer, TimeOnly value,
                                                JsonSerializerOptions options)
                => writer.WriteStringValue(value.ToString(serializationFormat));
        }




        public static string ToJsonString(this object value)
            => JsonSerializer.Serialize(value, SerializerOptions);
    }
}
