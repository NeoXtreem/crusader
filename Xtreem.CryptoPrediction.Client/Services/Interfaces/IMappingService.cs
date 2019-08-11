using System.Collections.Generic;

namespace Xtreem.CryptoPrediction.Client.Services.Interfaces
{
    public interface IMappingService<T1, T2> where T1 : class where T2 : class
    {
        T2 Map(T1 input);

        T1 Map(T2 input);

        IEnumerable<T2> Map(IEnumerable<T1> input);

        IEnumerable<T1> Map(IEnumerable<T2> input);
    }
}
