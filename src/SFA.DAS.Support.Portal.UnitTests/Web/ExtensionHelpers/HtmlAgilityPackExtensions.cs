using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using HtmlAgilityPack;

namespace SFA.DAS.Support.Portal.UnitTests.Web.ExtensionHelpers
{
    public static class HtmlAgilityPackExtensions
    {
        public static IHtmlDocument ToAngleSharp(this HtmlDocument document)
        {
            var html = document?.DocumentNode?.OuterHtml;
            return new HtmlParser().Parse(html ?? string.Empty);
        }
    }
}