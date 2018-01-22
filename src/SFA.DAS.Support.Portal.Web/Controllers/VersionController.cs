using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Infrastructure.Services;
using SFA.DAS.Support.Portal.Web.Models;
using SFA.DAS.Support.Shared;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Portal.Web.Controllers
{
    public sealed class VersionController : ApiController
    {
        private readonly IManifestRepository _repository;
        private readonly ILog _log;
        private readonly IWindowsLogonIdentityProvider _logonIdentityProvider;

        public VersionController(IManifestRepository repository, ILog log, IWindowsLogonIdentityProvider logonIdentityProvider)
        {
            _repository = repository;
            _log = log;
            _logonIdentityProvider = logonIdentityProvider;
        }

        // GET: api/Version
        [HttpGet]
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
        [HttpGet]
        [AllowAnonymous]
        public async Task<ICollection<SiteManifest>> Manifests()
        {
            try
            {
                var result = await _repository.GetManifests();
                
                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"{nameof(IManifestRepository)}.{nameof(IManifestRepository.GetManifests)}");
                throw;
            }
        }

        [Route("api/groups")]
        [HttpGet]
        [Authorize]
        public IEnumerable<string[]> GetGroups()
        {
            return from @group in _logonIdentityProvider.GetIdentity()?.Groups ?? new IdentityReferenceCollection()
                   select new string[] { @group.Value, @group.Translate(typeof(NTAccount)).ToString() };
        }

        [Route("api/claims")]
        [Authorize]
        [HttpGet]
        public IEnumerable<Claim> GetClaims()
        {
            return _logonIdentityProvider.GetIdentity()?.Claims ?? new List<Claim>();
        }

        [Route("api/test")]
        [Authorize(Roles = "ConsoleUser")]
        [HttpGet]
        public bool GetTest()
        {
            return true;
        }
    }
}