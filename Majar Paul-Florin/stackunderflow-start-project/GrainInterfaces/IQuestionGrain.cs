using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IQuestionGrain : IGrainWithIntegerKey
    {
        Task<string> GetStreamProvider(string message);
}
}
