using System;

namespace Xtreem.Crusader.Data.Models
{
    public class CurrencyPairChartPeriod
    {
        public CurrencyPairChart CurrencyPairChart { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
