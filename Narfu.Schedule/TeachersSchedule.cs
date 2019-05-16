using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Narfu.Common;
using Narfu.Domain.Entities;

namespace Narfu.Schedule
{
    public class TeachersSchedule
    {
        private readonly HttpClient _client;

        public TeachersSchedule(HttpClient client = null)
        {
            _client = client ?? HttpClientBuilder.BuildClient(new TimeSpan(0, 0, 5));
        }

        /// <summary>
        /// Получить перечисление с парами у указанного преподавателя
        /// </summary>
        /// <param name="siteTeacherId">ID преподавателя на сайте</param>
        /// <returns>Перечисление с парами у указанного преподавателя</returns>
        /// <exception cref="HttpRequestException">Выбрасывается, если сайт не вернул положительный Http код</exception>
        public async Task<IEnumerable<Lesson>> GetLessons(int siteTeacherId)
        {
            var response = await _client.GetAsync($"/?timetable&lecturer={siteTeacherId}");
            response.EnsureSuccessStatusCode();

            var doc = new HtmlDocument();
            doc.Load(await response.Content.ReadAsStreamAsync());

            var timetableNode = doc.DocumentNode.SelectNodes("//div[contains(@class, 'timetable_sheet')]");
            if(timetableNode is null)
            {
                throw new NullReferenceException("Can not find lessons on the page");
            }

            var teacher = doc.DocumentNode.SelectSingleNode("//a[@class='navbar-brand']/span[2]")
                             .GetNormalizedInnerText();

            var lessons = new List<Lesson>();
            foreach(var lessonNode in timetableNode)
            {
                if(lessonNode.ChildNodes.Count <= 3) // если пустая рамка с парой
                {
                    continue;
                }

                var date = lessonNode.ParentNode.SelectSingleNode(".//div[contains(@class,'dayofweek')]")
                                     .GetNormalizedInnerText()
                                     .Split(new[] { ',' }, 2)[1]
                                     .Trim();

                var adr = lessonNode.SelectSingleNode(".//span[@class='auditorium']")
                                    .GetNormalizedInnerText()
                                    .Split(new[] { ',' }, 2)
                                    .Select(x => x.Trim())
                                    .ToArray();

                var time = lessonNode.SelectSingleNode(".//span[@class='time_para']")
                                     .GetNormalizedInnerText()
                                     .Split(new[] { '–' }, 2);

                var groups = lessonNode.SelectSingleNode(".//span[@class='group']").GetNormalizedInnerText();
                var number = byte.Parse(lessonNode.SelectSingleNode(".//span[@class='num_para']")
                                                  .GetNormalizedInnerText());
                var lessonName = lessonNode.SelectSingleNode(".//span[@class='discipline']").GetNormalizedInnerText();
                var lessonType = lessonNode.SelectSingleNode(".//span[@class='kindOfWork']").GetNormalizedInnerText();

                lessons.Add(new Lesson
                {
                    Address = adr[1],
                    Auditory = adr[0],
                    Number = number,
                    Groups = groups,
                    Name = lessonName,
                    Type = lessonType,
                    Teacher = teacher,
                    StartTime = DateTime.ParseExact($"{date} {time[0]}", "dd.MM.yyyy HH:mm", null,
                                                    DateTimeStyles.AssumeLocal),
                    EndTime = DateTime.ParseExact($"{date} {time[1]}", "dd.MM.yyyy HH:mm", null,
                                                  DateTimeStyles.AssumeLocal)
                });
            }

            return lessons;
        }
    }
}