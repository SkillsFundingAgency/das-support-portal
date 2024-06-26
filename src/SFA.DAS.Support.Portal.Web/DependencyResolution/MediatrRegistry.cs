﻿using System.Diagnostics.CodeAnalysis;
using MediatR;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.Support.Portal.Web.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class MediatrRegistry : Registry
    {
        public MediatrRegistry()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>();
        }
    }
}