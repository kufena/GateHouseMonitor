using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GateHouseModel
{
    public class JsonIPAddressConverter : JsonConverter<IPAddress>
    {
        public override IPAddress Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) => IPAddress.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            IPAddress ipValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(ipValue.ToString());
    }
}
