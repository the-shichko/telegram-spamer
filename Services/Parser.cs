using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace telegram_spamer.Services
{
    public static class Parser
    {
        public static async Task<string> ParsePage(string url)
        {
            var client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            return await client.GetStringAsync(url);
        }

        public static IEnumerable<string> ParseText(string html, string tag, Regex regex = null)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc.DocumentNode.Descendants(tag)
                .Where(node => regex == null || regex.IsMatch(node.InnerText)).Select(node => node.InnerText).ToList();
        }
    }
}