using System;
using NUnit.Framework;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Domain
{
    public class WhenTestingEmployerUser
    {
        protected EmployerUser Unit;

        [SetUp]
        protected virtual void Setup()
        {
            Unit = Activator.CreateInstance<EmployerUser>();
        }
    }
}