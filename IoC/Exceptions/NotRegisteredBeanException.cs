using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.Exceptions
{
    public class NotRegisteredBeanException : ApplicationException
    {
        public NotRegisteredBeanException() : base()
        {
        }

        public NotRegisteredBeanException(string message) : base(message)
        {
        }
    }
}
