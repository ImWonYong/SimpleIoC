using IoC.Attr;
using IoC.Exceptions;
using IoC.ExtensionMethods;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IoC.Container
{
    public class BeanContainer
    {
        private readonly ConcurrentDictionary<Type, Lazy<object>> _beanMap = new ConcurrentDictionary<Type, Lazy<object>>();

        public void RegisterSingleton<T>() where T : class
        {
            var type = typeof(T);
            if (type.GetInterfaces().Length == 0)
            {
                throw new InterfaceNotImplementedException();
            }

            _beanMap.TryAdd(type.FirstInterface(), DependenciesInjectedBean(type));
        }

        public T GetService<T>() where T : class
        {
            if (_beanMap.TryGetValue(typeof(T), out var lazyResult))
            {
                return lazyResult.Value as T;
            }
            throw new NotRegisteredBeanException();
        }

        private Lazy<object> DependenciesInjectedBean(Type type)
        {
            var BINDING_FLAG = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            return new Lazy<object>(() =>
            {
                object result = default;
                try
                {
                    result = Activator.CreateInstance(type);
                }
                catch (MissingMethodException e)
                {
                    if (e.Message.Contains("Cannot create an instance of an interface"))
                    {
                        throw new NotRegisteredBeanException("Autowired Bean Was NOT Registered.");
                    }
                    throw e;
                }

                foreach (var prop in type.GetProperties(BINDING_FLAG).Where(each => each.HasAutowiredAttribute()))
                {
                    var dependency = _beanMap.GetOrAdd(prop.PropertyType, DependenciesInjectedBean(prop.PropertyType)).Value;
                    prop.SetValue(result, dependency);
                }

                foreach (var field in type.GetFields(BINDING_FLAG).Where(each => each.HasAutowiredAttribute()))
                {
                    var dependency = _beanMap.GetOrAdd(field.FieldType, DependenciesInjectedBean(field.FieldType)).Value;
                    field.SetValue(result, dependency);
                }

                return result;
            });
        }
    }
}
