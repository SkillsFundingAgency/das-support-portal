using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using System.Web.Mvc;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    public class WhenTestingAnApiControllerOfType<T> where T : ApiController
    {
        protected T Unit;
        protected ActionResult ActionResultResponse;

        [SetUp]
        public virtual void Setup()
        {
            Unit = System.Activator.CreateInstance<T>();
        }
    }
}