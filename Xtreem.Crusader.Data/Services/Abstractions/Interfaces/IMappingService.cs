using AutoMapper;

namespace Xtreem.Crusader.Data.Services.Abstractions.Interfaces
{
    public interface IMappingService
    {
        IMapper GetMapper<TProfile>() where TProfile : Profile, new();
    }
}
