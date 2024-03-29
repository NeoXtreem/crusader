﻿using JetBrains.Annotations;

namespace Xtreem.Crusader.Shared.Models
{
    public class DataOptions
    {
        [UsedImplicitly]
        public string Endpoint { get; set; }

        [UsedImplicitly]
        public string PrimaryKey { get; set; }

        [UsedImplicitly]
        public string MarketDataDb { get; set; }

        [UsedImplicitly]
        public string HistoricalOhlcvContainer { get; set; }
    }
}
