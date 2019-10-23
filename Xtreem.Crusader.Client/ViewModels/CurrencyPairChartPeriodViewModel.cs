using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Xtreem.Crusader.Client.ViewModels
{
    [PublicAPI]
    public class CurrencyPairChartPeriodViewModel : IValidatableObject
    {
        [Required]
        public string CurrencyPairBaseCurrency { get; set; }

        [Required]
        public string CurrencyPairQuoteCurrency { get; set; }

        [Required]
        public string Resolution { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateTime.Compare(From, To) > 0)
            {
                yield return new ValidationResult("Invalid date range specified.", new[] {nameof(From), nameof(To)});
            }
        }
    }
}
