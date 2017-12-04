using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    public class WhenTestingAnMvcControllerOfType<T> where T : Controller
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
