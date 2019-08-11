using MongoDB.Bson.Serialization.Attributes;
using Xtreem.CryptoPrediction.Data.Types;

namespace Xtreem.CryptoPrediction.Data.Models
{
    [BsonIgnoreExtraElements]
    public class Ohlcv
    {
        public string Base { get; set; }

        public string Quote { get; set; }

        public string Resolution { get; set; }

        public long Time { get; set; }

        public decimal Open { get; set; }

        public decimal Close { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal VolumeFrom { get; set; }

        public decimal VolumeTo { get; set; }
    }
}
