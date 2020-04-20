using System;

namespace Max.PetMini.Extension.Exceptions
{
    public class NoPermissionException : ApplicationException
    {

        public NoPermissionException() { }

        public NoPermissionException(string message) : base(message)
        {

        }
    }
}
