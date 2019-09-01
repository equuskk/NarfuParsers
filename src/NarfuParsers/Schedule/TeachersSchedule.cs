using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using HtmlAgilityPack;
using NarfuParsers.Common;
using NarfuParsers.Entities;
using NarfuParsers.Extensions;

namespace NarfuParsers.Schedule
{
    public class TeachersSchedule
    {
        private readonly IFlurlRequest _client;

        public TeachersSchedule(TimeSpan timeout)
        {
            _client = FlurlClientBuilder.BuildClient(timeout);
        }

        /// <summary>
        ///     Получить перечисление с парами у указанного преподавателя
        /// </summary>
        /// <param name="siteTeacherId">ID преподавателя на сайте</param>
        /// <returns>Перечисление с парами у указанного преподавателя</returns>
        /// <exception cref="FlurlHttpException">Выбрасывается, если сайт не вернул положительный Http код</exception>
        public async Task<IEnumerable<Lesson>> GetLessons(int siteTeacherId)
        {
            var response = await _client
                                 .SetQueryParam("timetable")
                                 .SetQueryParam("lecturer", siteTeacherId)
                                 .GetAsync();

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

                var adr = lessonNode.SelectSingleNode(".//span[contains(@class,'auditorium')]")
                                    .GetNormalizedInnerText()
                                    .Split(new[] { ',' }, 2)
                                    .Select(x => x.Trim())
                                    .ToArray();

                var time = lessonNode.SelectSingleNode(".//span[contains(@class,'time_para')]")
                                     .GetNormalizedInnerText()
                                     .Split(new[] { '–' }, 2);
                    
                var groups = lessonNode.SelectSingleNode(".//span[contains(@class,'group')]").GetNormalizedInnerText();
                var number = byte.Parse(lessonNode.SelectSingleNode(".//span[contains(@class,'num_para')]")
                                                  .GetNormalizedInnerText()); 
                var lessonName = lessonNode.SelectSingleNode(".//span[contains(@class,'discipline')]").GetNormalizedInnerText();
                var lessonType = lessonNode.SelectSingleNode(".//span[contains(@class,'kindOfWork')]").GetNormalizedInnerText();

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