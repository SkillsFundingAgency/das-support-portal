﻿using System.Collections.Generic;
using NUnit.Framework;

namespace SFA.DAS.Support.Portal.UnitTests.Web.Controllers.Version
{
    public class WhenCallingTheVersionControllerGetGroupsMethod : WithAPreparedVersionController
    {
        [Test]
        public void ItShouldReturnThelistOfGroups()
        {
           
            var result = Unit.GetGroups();
            Assert.IsInstanceOf<IEnumerable<string[]>>(result);
            CollectionAssert.IsEmpty(result);
        }
    }
}