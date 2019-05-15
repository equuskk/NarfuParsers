using System.Net;
using HtmlAgilityPack;

namespace Narfu.Schedule
{
    internal static class HtmlNodeExtension
    {
        internal static string GetNormalizedInnerText(this HtmlNode node)
        {
            return WebUtility.HtmlDecode(node.InnerText
                                             .Trim()
                                             .Replace("\n", ""));
        }
    }
}