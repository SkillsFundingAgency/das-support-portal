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
        private readonly WindowsIdentity _requestLogonUserIdentity;
        private readonly IWindowsLogonIdentityProvider _logonIdentityProvider;

        public VersionController(IManifestRepository repository, ILog log, IWindowsLogonIdentityProvider logonIdentityProvider)
        {
            _repository = repository;
            _log = log;
            _logonIdentityProvider = logonIdentityProvider;
            _requestLogonUserIdentity = _logonIdentityProvider.GetIdentity();
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

        [System.Web.Http.Route("api/manifest")]
        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.AllowAnonymous]
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

        [System.Web.Http.Route("api/groups")]
        [System.Web.Http.Authorize]
        public IEnumerable<string[]> GetGroups()
        {
            foreach (IdentityReference group in _requestLogonUserIdentity?.Groups?? new IdentityReferenceCollection())
            {
                yield return new string[] {group.Value, group.Translate(typeof(NTAccount)).ToString()};
            }
        }

        [System.Web.Http.Route("api/claims")]
        //[Authorize]
        public IEnumerable<Claim> GetClaims()
        {
            return _requestLogonUserIdentity?.Claims?? new List<Claim>();
        }

        [System.Web.Http.Route("api/test")]
        [System.Web.Http.Authorize(Roles = "ConsoleUser")]
        public bool GetTest()
        {
            return true;
        }
    }
}