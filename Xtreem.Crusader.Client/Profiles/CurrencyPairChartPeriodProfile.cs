using AutoMapper;
using Xtreem.Crusader.Client.ViewModels;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Types;

namespace Xtreem.Crusader.Client.Profiles
{
    public class CurrencyPairChartPeriodProfile : Profile
    {
        public CurrencyPairChartPeriodProfile()
        {
            CreateMap<CurrencyPairChartPeriod, CurrencyPairChartPeriodViewModel>()
                .IncludeMembers(s => s.CurrencyPairChart, s => s.DateTimeInterval)
                .ReverseMap()
                .ForPath(d => d.CurrencyPairChart.Resolution, o => o.MapFrom(s => Resolution.Parse(s.Resolution)));

            CreateMap<CurrencyPairChart, CurrencyPairChartPeriodViewModel>(MemberList.None)
                .ReverseMap()
                .ForMember(d => d.Resolution, o => o.Ignore());

            CreateMap<DateTimeInterval, CurrencyPairChartPeriodViewModel>(MemberList.None)
                .ReverseMap();
        }
    }
}
