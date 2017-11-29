using System.Web.Mvc;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    public class WhenTestingAController <T> where T: Controller
    {
        protected T Unit;
        protected ActionResult _actionResultResponse;

        [SetUp]
        public virtual void Setup()
        {
            Unit = System.Activator.CreateInstance<T>();

        }
    }
}
