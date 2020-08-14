﻿using System.IO;
using Binaron.Serializer;
using Neo.LocationSearch.Indexes.Models;
using Newtonsoft.Json;

namespace Neo.LocationSearch.Indexes
{
    public class GeoMapConstructor : IGeoMapConstructor
    {
        public IGeoMap FromJsonFile(string dataFile)
        {
            return new GeoMapIndex(JsonConvert.DeserializeObject<GeoMapData>(File.ReadAllText(dataFile)));
        }

        public IGeoMap FromBinaryFile(string dataFile)
        {
            return new GeoMapIndex(BinaronConvert.Deserialize<GeoMapData>(File.OpenRead(dataFile)));
        }
    }
}