using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.Core.Domain.Exceptions
{
    [ExcludeFromCodeCoverage]
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