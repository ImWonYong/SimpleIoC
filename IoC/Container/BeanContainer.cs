using IoC.Attr;
using IoC.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IoC.Container
{
    /// <summary>
    /// TODO
    ///     1) improve performance > 
    ///     2) test Get/Register in multi-thread environment
    ///     3) Add Feature - RegisterTransient 
    ///     4) Add Factory Bean
    /// </summary>
    public class BeanContainer
    {
        private readonly ConcurrentDictionary<Type, Lazy<object>> _beanStorage = new ConcurrentDictionary<Type, Lazy<object>>();

        public void RegisterSingleton<T>() where T : class
        {
            var type = typeof(T);
            if (type.GetInterfaces().Length == 0)
            {
                throw new InterfaceNotImplementedException();
            }

            _beanStorage.TryAdd(FirstInterface(type), DependenciesInjectedBean(type));
        }

        public T GetService<T>() where T : class
        {
            if (_beanStorage.TryGetValue(typeof(T), out var result))
            {
                return result.Value as T;
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

                foreach (var prop in type.GetProperties(BINDING_FLAG).Where(each => HasAutowiredAttribute(each)))
                {
                    var dependency = _beanStorage.GetOrAdd(prop.PropertyType, DependenciesInjectedBean(prop.PropertyType)).Value;
                    prop.SetValue(result, dependency);
                }

                foreach (var field in type.GetFields(BINDING_FLAG).Where(each => HasAutowiredAttribute(each)))
                {
                    var dependency = _beanStorage.GetOrAdd(field.FieldType, DependenciesInjectedBean(field.FieldType)).Value;
                    field.SetValue(result, dependency);
                }

                return result;
            });
        }

        private static bool HasAutowiredAttribute(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<AutowiredAttribute>() != null;
        }

        private static bool HasAutowiredAttribute(FieldInfo fieldInfo)
        {
            return fieldInfo.GetCustomAttribute<AutowiredAttribute>() != null;
        }

        private static Type FirstInterface(Type type)
        {
            return type.GetInterfaces().First();
        }
    }
}