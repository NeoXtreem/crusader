using AutoMapper;

namespace Xtreem.Crusader.Data.Services.Interfaces
{
    public interface IMappingService
    {
        IMapper GetMapper<TProfile>() where TProfile : Profile, new();
    }
}
