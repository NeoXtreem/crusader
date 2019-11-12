using System;
using System.Collections.Generic;
using AutoMapper;
using JetBrains.Annotations;
using Xtreem.Crusader.Data.Services.Interfaces;
using Xtreem.Crusader.Utilities.Attributes;

namespace Xtreem.Crusader.Data.Services
{
    [Inject, UsedImplicitly]
    public class MappingService : IMappingService
    {
        private static readonly Dictionary<Type, IMapper> Mappers = new Dictionary<Type, IMapper>();

        public IMapper GetMapper<TProfile>() where TProfile : Profile, new()
        {
            if (!Mappers.TryGetValue(typeof(TProfile), out var mapper))
            {
                Mappers.Add(typeof(TProfile), mapper = new MapperConfiguration(cfg => { cfg.AddProfile<TProfile>(); }).CreateMapper());
            }

            return mapper;
        }
    }
}
