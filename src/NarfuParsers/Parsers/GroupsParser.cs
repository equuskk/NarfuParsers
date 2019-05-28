using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl.Http;
using HtmlAgilityPack;
using NarfuParsers.Common;
using Group = NarfuParsers.Entities.Group;

namespace NarfuParsers.Parsers
{
    public class GroupsParser
    {
        private readonly IFlurlRequest _client;

        public GroupsParser(TimeSpan timeout)
        {
            _client = FlurlClientBuilder.BuildClient(timeout);
        }

        /// <summary>
        ///     Получить список групп из указанной высшей школы
        /// </summary>
        /// <param name="schoolId">ID высшей школы</param>
        /// <returns>Перечисления групп из ВШ</returns>
        /// <exception cref="FlurlHttpException">Выбрасывается, если сайт не вернул положительный Http код</exception>
        public async Task<IEnumerable<Group>> GetGroupsFromSchool(int schoolId)
        {
            var response = await _client
                                 .SetQueryParam("groups")
                                 .SetQueryParam("institution", schoolId)
                                 .GetAsync();

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