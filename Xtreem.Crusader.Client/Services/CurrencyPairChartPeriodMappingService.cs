using AutoMapper;
using Xtreem.Crusader.Client.Services.Interfaces;
using Xtreem.Crusader.Client.ViewModels;
using Xtreem.Crusader.Data.Models;
using Xtreem.Crusader.Data.Services.Abstractions;
using Xtreem.Crusader.Data.Types;

namespace Xtreem.Crusader.Client.Services
{
    public class CurrencyPairChartPeriodMappingService : MappingService<CurrencyPairChartPeriodViewModel, CurrencyPairChartPeriod>, ICurrencyPairChartPeriodMappingService
    {
        public CurrencyPairChartPeriodMappingService()
        {
            Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CurrencyPairChartPeriod, CurrencyPairChartPeriodViewModel>().IncludeMembers(s => s.CurrencyPairChart).ReverseMap()
                    .ForPath(d => d.CurrencyPairChart.Resolution, o => o.MapFrom(s => Resolution.Parse(s.Resolution)));
                cfg.CreateMap<CurrencyPairChart, CurrencyPairChartPeriodViewModel>(MemberList.None).ReverseMap()
                    .ForMember(d => d.Resolution, o => o.Ignore());
            }).CreateMapper();
        }
    }
}
