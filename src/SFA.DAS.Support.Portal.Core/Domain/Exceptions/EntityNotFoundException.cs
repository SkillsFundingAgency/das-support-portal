using System;

namespace SFA.DAS.Support.Portal.Core.Domain.Exceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException(string message, object identifier, Exception innerException) : base(message,
            innerException)
        {
            Identifier = identifier;
        }

        public object Identifier { get; set; }
    }
}