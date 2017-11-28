using AngleSharp.Parser.Html;
using SFA.DAS.Support.Portal.ApplicationServices.Services;

namespace SFA.DAS.Support.Portal.Infrastructure.Services
{
    public class FormMapper : IFormMapper
    {
        public string UpdateForm(string key, string id, string url, string html)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(html);
            var form = document.QuerySelector("form");
            var innerAction = form.Attributes["action"].Value;
            form.SetAttribute("action", $"/resource/challenge?resourceId={id}&key={key}");

            form.AppendChild(document.CreateHidden("challengeKey", key));
            form.AppendChild(document.CreateHidden("innerAction", innerAction));
            form.AppendChild(document.CreateHidden("redirect", url));

            return document.DocumentElement.OuterHtml;
        }
    }
}