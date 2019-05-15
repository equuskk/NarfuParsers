using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Narfu.Common;
using Group = Narfu.Domain.Entities.Group;

namespace Narfu.Parsers
{
    public class GroupsParser
    {
        private readonly HttpClient _client;

        public GroupsParser(HttpClient client = null)
        {
            _client = client ?? HttpClientBuilder.BuildClient(new TimeSpan(0, 0, 5));
        }

        public async Task<IEnumerable<Group>> GetGroupsFromSchool(int groupId)
        {
            var response = await _client.GetAsync($"?groups&institution={groupId}");
            response.EnsureSuccessStatusCode();

            var doc = new HtmlDocument();
            doc.Load(await response.Content.ReadAsStreamAsync());

            var groups = doc.DocumentNode.SelectNodes("//div[contains(@class, 'tab-pane')]/div/a")
                            .Where(x => x.Attributes["href"].Value.StartsWith("?") && int.TryParse(x.ChildNodes[1].InnerText, out _))
                            .Select(x => new Group
                            {
                                RealId = int.Parse(x.ChildNodes[1].InnerText),
                                SiteId = int.Parse(x.Attributes["href"].Value.Split('=')[1]),
                                Name = Regex.Replace(x.ChildNodes[2].InnerText.Trim(), @"\s+", " ")
                            })
                            .Distinct()
                            .ToArray();

            return groups;
        }
    }
}