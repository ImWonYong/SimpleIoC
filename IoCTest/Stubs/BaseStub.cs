using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCTest.Stubs
{
    public interface IBaseStub
    {
        int GetValue();
    }

    internal class BaseStub : IBaseStub
    {
        public const int VALUE = 42;

        public int GetValue() => VALUE;
    }
}
