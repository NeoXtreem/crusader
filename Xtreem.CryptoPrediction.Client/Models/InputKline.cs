using System;

namespace Xtreem.CryptoPrediction.Client.Models
{
    public class InputKline
    {
        public DateTime OpenTime { get; set; }

        public float Open { get; set; }

        public float High { get; set; }

        public float Low { get; set; }

        public float Close { get; set; }

        public float Volume { get; set; }

        public DateTime CloseTime { get; set; }

        public float QuoteAssetVolume { get; set; }

        public float TradeCount { get; set; }

        public float TakerBuyBaseAssetVolume { get; set; }

        public float TakerBuyQuoteAssetVolume { get; set; }
    }
}
