using IoC.Attr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IoC.ExtensionMethods
{
    internal static class PropertyInfoExtensionMethods
    {
        public static bool HasAutowiredAttribute(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<AutowiredAttribute>() != null;
        }
    }

    internal static class FieldInfoExtensionMethods
    {
        public static bool HasAutowiredAttribute(this FieldInfo fieldInfo)
        {
            return fieldInfo.GetCustomAttribute<AutowiredAttribute>() != null;
        }
    }

    internal static class TypeExtensionMethods
    {
        public static Type FirstInterface(this Type type)
        {
            return type.GetInterfaces().First();
        }
    }


}
