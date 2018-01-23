using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using System.Web.Mvc;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    public class WhenTestingAnApiControllerOfType<T> where T : ApiController
    {
        protected ActionResult ActionResultResponse;
        protected T Unit;

        [SetUp]
        public virtual void Setup()
        {
            Unit = Activator.CreateInstance<T>();
        }
    }
}