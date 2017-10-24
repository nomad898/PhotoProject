using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.BLL.Util
{
    public class OperationDetails
    {
        public bool Succeeded { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }

        public OperationDetails(bool succedeed,
            string message,
            string property)
        {
            Succeeded = succedeed;
            Message = message;
            Property = property;
        }
    }
}
