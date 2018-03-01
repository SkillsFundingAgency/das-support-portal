using AngleSharp.Parser.Html;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Shared.Discovery;
using System;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    public class FormMapper : IFormMapper
    {
        private readonly ILog _log;

        public FormMapper(ILog log)
        {
            _log = log;
        }

        public string UpdateForm(SupportServiceResourceKey resourceKey, SupportServiceResourceKey challengeKey, string id, string url, string html)
        {
            try
            {
                _log.Info($"Html: {html} - id : {id} - url : {url}");

                var parser = new HtmlParser();
                var document = parser.Parse(html);
                var form = document.QuerySelector("form");
                var innerAction = form.Attributes["action"].Value;
                form.SetAttribute("action", $"/resource/challenge?resourceId={id}&resourceKey={resourceKey}&challengeKey={challengeKey}");

                form.AppendChild(document.CreateHidden("resourceKey", resourceKey.ToString()));
                form.AppendChild(document.CreateHidden("challengeKey", challengeKey.ToString()));
                form.AppendChild(document.CreateHidden("innerAction", innerAction));
                form.AppendChild(document.CreateHidden("redirect", url));
                return document
                        .DocumentElement
                        .InnerHtml
                        .Replace("<head>", "").Replace("</head>", "")
                        .Replace("<body>", "").Replace("</body>", "");
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error while updating challenge Form Attributes");

                throw;
            }

        }
    }
}