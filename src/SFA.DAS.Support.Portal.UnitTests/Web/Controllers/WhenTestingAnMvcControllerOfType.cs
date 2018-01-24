using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    [ExcludeFromCodeCoverage]
    public class WhenTestingAnMvcControllerOfType<T> where T : Controller
    {
        protected ActionResult ActionResultResponse;
        protected T Unit;

        /// <summary>
        ///     Override if you require a non default constructor setup
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            Unit = Activator.CreateInstance<T>();
        }
    }
}