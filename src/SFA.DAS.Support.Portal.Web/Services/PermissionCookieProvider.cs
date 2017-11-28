using System;
using System.Security.Principal;
using System.Web;
using Newtonsoft.Json;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Portal.Web.Models;
using SFA.DAS.Support.Portal.Web.Settings;

namespace SFA.DAS.Support.Portal.Web.Services
{
    public class PermissionCookieProvider : ICheckPermissions, IGrantPermissions
    {
        private readonly ICrypto _crypto;
        private readonly IChallengeSettings _settings;
        private readonly IRoleSettings _roleSettings;
        private readonly string _cookieFormat = "Elevate[{0}]";

        public PermissionCookieProvider(ICrypto crypto, IChallengeSettings settings, IRoleSettings roleSettings)
        {
            _crypto = crypto;
            _settings = settings;
            _roleSettings = roleSettings;
        }

        public bool HasPermissions(HttpRequestBase request, HttpResponseBase response, IPrincipal user, string id)
        {
            var debugModeT2 = System.Diagnostics.Debugger.IsAttached && _roleSettings.ForceT2UserLocally;
            var t2User = user.Identity.IsAuthenticated && user.IsInRole(_roleSettings.T2Role);

            if (debugModeT2 || t2User)
            {
                return true;
            }

            var httpCookie = request.Cookies.Get(string.Format(_cookieFormat, id.ToLower()));
            if (httpCookie == null)
            {
                return false;
            }

            var payload = GetPayload(httpCookie.Value);

            if (payload != null && payload.EndDate > DateTime.UtcNow && string.Equals(id, payload.Id, StringComparison.InvariantCultureIgnoreCase))
            {
                GivePermissions(response, user, id);
                return true;
            }

            return false;
        }

        public void GivePermissions(HttpResponseBase response, IPrincipal user, string id)
        {
            var payload = CreatePayload(id, user);
            var name = string.Format(_cookieFormat, id.ToLower());
            var httpCookie = response.Cookies.Get(name);
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie(name, payload);
            }
            else
            {
                httpCookie.Value = payload;
            }

            response.Cookies.Add(httpCookie);
        }

        public string CreatePayload(string id, IPrincipal user)
        {
            var model = new PermissionCookieModel
            {
                Id = id,
                EndDate = DateTime.UtcNow.AddMinutes(_settings.ChallengeTimeoutMinutes)
            };

            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return _crypto.EncryptStringAES(json);
        }

        public PermissionCookieModel GetPayload(string payload)
        {
            var json = _crypto.DecryptStringAES(payload);
            return JsonConvert.DeserializeObject<PermissionCookieModel>(json);
        }
    }
}