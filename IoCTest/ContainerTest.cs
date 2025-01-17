using IoC.Container;
using IoC.Exceptions;
using IoCTest.Mocks;
using IoCTest.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IoCTest
{
    [TestClass]
    public class ContainerTest
    {
        private BeanContainer container;

        [TestInitialize]
        public void SetUpBeforeEach()
        {
            container = new BeanContainer();
        }


        [TestMethod]
        public void AddAndGetNotHavingAutowired()
        {
            container.RegisterSingleton<BaseStub>();

            var service = container.GetService<IBaseStub>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(IBaseStub));
            Assert.IsTrue(service.GetValue() == BaseStub.VALUE);
        }


        [TestMethod]
        public void AddAndGetHavingAutowired()
        {
            container.RegisterSingleton<StubHavingAutowired>();
            container.RegisterSingleton<BaseStub>();

            var havingAutowiredStub = container.GetService<IStubHavingAutowired>();

            Assert.IsNotNull(havingAutowiredStub);
            Assert.IsInstanceOfType(havingAutowiredStub, typeof(IStubHavingAutowired));
            Assert.IsTrue(havingAutowiredStub.GetSubStubValue() == BaseStub.VALUE);
            Assert.IsTrue(havingAutowiredStub.GetValue() == BaseStub.VALUE + StubHavingAutowired.OFFSET_VALUE);

            var injectedStub = container.GetService<IBaseStub>();
            Assert.IsNotNull(injectedStub);
            Assert.IsInstanceOfType(injectedStub, typeof(IBaseStub));
            Assert.IsTrue(injectedStub.GetValue() == BaseStub.VALUE);
        }


        [TestMethod]
        public void AddServiceNotImplementingInterface_ItThrow()
        {
            var stub = new StubNotImplementingInterface();

            Assert.ThrowsException<InterfaceNotImplementedException>(() =>
            {
                container.RegisterSingleton<StubNotImplementingInterface>();
            });
        }


        [TestMethod]
        public void GetNotRegisteredBean_ItThrow()
        {
            Assert.ThrowsException<NotRegisteredBeanException>(() =>
            {
                container.GetService<IBaseStub>();
            });
        }


        [TestMethod]
        public void GetServiceOnlyParentBeanRegistered_ItThrow()
        {
            container.RegisterSingleton<StubHavingAutowired>();

            Assert.ThrowsException<NotRegisteredBeanException>(() =>
            {
                var havingAutowiredStub = container.GetService<IStubHavingAutowired>();
            });
        }
    }
}
