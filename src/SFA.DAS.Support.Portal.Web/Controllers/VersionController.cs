using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Portal.Web.Controllers
{
    public sealed class VersionController : ApiController
    {
        private readonly IManifestRepository _repository;
        private readonly ILog _log;

        public VersionController(IManifestRepository repository, ILog log)
        {
            _repository = repository;
            _log = log;
        }

        // GET: api/Version
        public VersionInformation Get()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string assemblyInformationalVersion = fileVersionInfo.ProductVersion;
            return new VersionInformation
            {
                Version = assemblyInformationalVersion,
                AssemblyVersion = version
            };
        }

        [Route("api/manifest")]
        [AcceptVerbs("GET")]
        [AllowAnonymous]
        public ICollection<SiteManifest> Manifests()
        {
            try
            {
                return _repository.Manifests;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Get manifests");
                throw;
            }
        }

        [Route("api/groups")]
        [Authorize]
        public IEnumerable<string[]> GetGroups()
        {
            foreach (IdentityReference group in System.Web.HttpContext.Current.Request.LogonUserIdentity.Groups)
            {
                yield return new string[] {group.Value, group.Translate(typeof(NTAccount)).ToString()};
            }
        }

        [Route("api/claims")]
        [Authorize]
        public IEnumerable<Claim> GetClaims()
        {
            return System.Web.HttpContext.Current.Request.LogonUserIdentity.Claims;
        }

        [Route("api/test")]
        [Authorize(Roles = "ConsoleUser")]
        public bool GetTest()
        {
            return true;
        }
    }

    public class VersionInformation
    {
        public string AssemblyVersion { get; set; }
        public string Version { get; set; }
    }
}