using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NarfuParsers.Common;
using NarfuParsers.Entities;

namespace NarfuParsers.Parsers
{
    public class SchoolsParser
    {
        private readonly HttpClient _client;

        public SchoolsParser(HttpClient client = null)
        {
            _client = client ?? HttpClientBuilder.BuildClient(TimeSpan.FromSeconds(5));
        }

        /// <summary>
        ///     Получить перечисление высших школ
        /// </summary>
        /// <returns>Перечисление с высшими школами</returns>
        /// <exception cref="HttpRequestException">Выбрасывается, если сайт не вернул положительный Http код</exception>
        public async Task<IEnumerable<School>> GetSchools()
        {
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();

            var doc = new HtmlDocument();
            doc.Load(await response.Content.ReadAsStreamAsync());

            var schools = doc.DocumentNode
                             .SelectNodes("//div[@id='classic']/div[contains(@class, 'institution_button')]/a")
                             .Select(x => new School
                             {
                                 Id = int.Parse(x.Attributes["href"].Value.Split('=')[1]),
                                 Url = $"{Constants.EndPoint}{x.Attributes["href"].Value}",
                                 Name = x.InnerText.Trim()
                             })
                             .Distinct();

            return schools;
        }
    }
}