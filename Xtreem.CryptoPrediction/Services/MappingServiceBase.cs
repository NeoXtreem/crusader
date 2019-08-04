using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Xtreem.CryptoPrediction.Service.Services
{
    public abstract class MappingServiceBase<T1, T2> where T1 : class where T2 : class
    {
        protected IMapper Mapper { get; set; }

        protected void InitialiseMapper()
        {
            Mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T1, T2>();
                cfg.CreateMap<T2, T1>();

            }).CreateMapper();
        }

        public T2 Map(T1 input) => Mapper.Map<T1, T2>(input);

        public T1 Map(T2 input) => Mapper.Map<T2, T1>(input);

        public IEnumerable<T2> Map(IEnumerable<T1> input) => input.Select(Map);

        public IEnumerable<T1> Map(IEnumerable<T2> input) => input.Select(Map);
    }
}