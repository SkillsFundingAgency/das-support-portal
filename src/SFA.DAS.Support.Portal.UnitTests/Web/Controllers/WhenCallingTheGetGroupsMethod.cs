using System.Collections.Generic;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers
{
    public class WhenCallingTheGetGroupsMethod : WithAPreparedVersionController
    {
        [Test]
        public void ItShouldReturnThelistOfGroups()
        {
           
            var result = Unit.GetGroups();
            Assert.IsInstanceOf<IEnumerable<string[]>>(result);
            Assert.Fail("TODO: Iain Mock Groups and test those groups returned");
        }
    }
}