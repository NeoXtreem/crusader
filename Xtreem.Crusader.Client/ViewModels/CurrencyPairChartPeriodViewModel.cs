using System;
using JetBrains.Annotations;

namespace Xtreem.Crusader.Client.ViewModels
{
    [PublicAPI]
    public class CurrencyPairChartPeriodViewModel
    {
        public string CurrencyPairBaseCurrency { get; set; }

        public string CurrencyPairQuoteCurrency { get; set; }

        public string Resolution { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
