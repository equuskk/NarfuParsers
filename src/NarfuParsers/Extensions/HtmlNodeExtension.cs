using System.Net;
using HtmlAgilityPack;

namespace NarfuParsers.Extensions
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