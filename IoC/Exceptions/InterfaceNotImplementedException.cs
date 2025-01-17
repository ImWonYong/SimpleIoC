using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.Exceptions
{
    public class InterfaceNotImplementedException : ApplicationException
    {
        public InterfaceNotImplementedException() : base()
        {
            
        }

        public InterfaceNotImplementedException(string errorMessage) : base(errorMessage) 
        {
        }
    }
}
