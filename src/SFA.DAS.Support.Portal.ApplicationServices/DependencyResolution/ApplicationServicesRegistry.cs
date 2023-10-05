﻿using System.Diagnostics.CodeAnalysis;
using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace SFA.DAS.Support.Portal.ApplicationServices.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public sealed class ApplicationServicesRegistry : Registry
    {
        public ApplicationServicesRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssemblyContainingType<ApplicationServicesRegistry>();
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.AddAllTypesOf(typeof(IRequestHandler<,>));
                });
            For<ICrypto>().Use<Crypto>();
            For<IChallengeService>().Use<ChallengeService>();
            For<IDatetimeService>().Use<DatetimeService>();
            For<IManifestRepository>().Singleton().Use<ManifestRepository>();
        }
    }
}