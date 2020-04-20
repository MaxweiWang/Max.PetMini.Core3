using System;

namespace Max.PetMini.Extension.Exceptions
{
    public class BusinessException : ApplicationException
    {

        public BusinessException() { }

        public BusinessException(string message) : base(message)
        {

        }
    }
}
