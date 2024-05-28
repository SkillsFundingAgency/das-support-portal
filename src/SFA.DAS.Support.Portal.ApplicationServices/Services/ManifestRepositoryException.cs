using System;

namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public class ManifestRepositoryException : Exception
    {
        public ManifestRepositoryException(string message) : base(message) { }
    }
}