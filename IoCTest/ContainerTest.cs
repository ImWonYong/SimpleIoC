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
        public void InitBeforeEachTest()
        {
            container = new BeanContainer();
        }


        [TestMethod]
        public void AddAndGet()
        {
            container.RegisterSingleton<BaseStub>();

            var service = container.GetService<IBaseStub>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(IBaseStub));
            Assert.IsTrue(service.GetValue() == 42);
        }


        [TestMethod]
        public void AddAndGetServiceHavingAutowired()
        {
            container.RegisterSingleton<HavingAutowiredStub>();
            container.RegisterSingleton<BaseStub>();

            var havingAutowiredStub = container.GetService<IHavingAutowiredStub>();

            Assert.IsNotNull(havingAutowiredStub);
            Assert.IsInstanceOfType(havingAutowiredStub, typeof(IHavingAutowiredStub));
            Assert.IsTrue(havingAutowiredStub.GetSubStubValue() == BaseStub.VALUE);
            Assert.IsTrue(havingAutowiredStub.GetValue() == BaseStub.VALUE + HavingAutowiredStub.OFFSET_VALUE);

            var injectedStub = container.GetService<IBaseStub>();
            Assert.IsNotNull(injectedStub);
            Assert.IsInstanceOfType(injectedStub, typeof(IBaseStub));
            Assert.IsTrue(injectedStub.GetValue() == BaseStub.VALUE);
        }


        [TestMethod]
        public void AddServiceNotImplementingInterface_ItThrow()
        {
            var stubService = new NotImplementingInterfaceStub();

            Assert.ThrowsException<InterfaceNotImplementedException>(() =>
            {
                container.RegisterSingleton<NotImplementingInterfaceStub>();
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
            container.RegisterSingleton<HavingAutowiredStub>();

            Assert.ThrowsException<NotRegisteredBeanException>(() =>
            {
                var havingAutowiredStub = container.GetService<IHavingAutowiredStub>();
            });
        }
    }
}
