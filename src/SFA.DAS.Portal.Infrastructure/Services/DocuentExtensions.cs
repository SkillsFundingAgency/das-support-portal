using AngleSharp.Dom;
using AngleSharp.Dom.Html;

namespace SFA.DAS.Portal.Infrastructure.Services
{
    public static class DocuentExtensions
    {
        public static INode CreateHidden(this IHtmlDocument document, string name, string value)
        {
            var challengeAttr = document.CreateElement("input");
            document.CreateElement("input");
            challengeAttr.SetAttribute("name", name);
            challengeAttr.SetAttribute("value", value);
            challengeAttr.SetAttribute("type", "hidden");
            return challengeAttr;
        }
    }
}