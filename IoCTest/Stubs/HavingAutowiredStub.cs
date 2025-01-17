﻿using IoC.Attr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace IoCTest.Stubs
{
    internal interface IHavingAutowiredStub
    {
        int GetSubStubValue();
        int GetValue();
    }

    internal class HavingAutowiredStub : IHavingAutowiredStub
    {
        public const int OFFSET_VALUE = 42;

        [Autowired]
        private IBaseStub subStub;

        public int GetSubStubValue() => subStub.GetValue();

        public int GetValue() => subStub.GetValue() + OFFSET_VALUE;
    }
}
