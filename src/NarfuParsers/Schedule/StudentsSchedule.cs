using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Ical.Net;
using NarfuParsers.Common;
using NarfuParsers.Entities;

namespace NarfuParsers.Schedule
{
    public class StudentsSchedule
    {
        private readonly IFlurlRequest _client;

        public StudentsSchedule(TimeSpan timeout)
        {
            _client = FlurlClientBuilder.BuildClient(timeout);
        }

        /// <summary>
        ///     Получить перечисление с парами в указанной группе
        /// </summary>
        /// <param name="siteGroupId">ID группы на сайте</param>
        /// <param name="from">Дата, с которой необходимо получить расписание</param>
        /// <returns>Перечисление с парами</returns>
        /// <exception cref="FlurlHttpException">Выбрасывается, если сайт не вернул положительный Http код</exception>
        public async Task<IEnumerable<Lesson>> GetLessons(int siteGroupId, DateTime from = default)
        {
            if(from == default)
            {
                from = DateTime.Today;
            }

            var response = await _client
                                 .SetQueryParam("icalendar")
                                 .SetQueryParam("oid", siteGroupId)
                                 .SetQueryParam("from", from.ToString("dd.MM.yyyy"))
                                 .GetAsync();

            var calendar = Calendar.Load(await response.Content.ReadAsStreamAsync());
            var events = calendar.Events
                                 .Distinct()
                                 .OrderBy(x => x.DtStart.Value);

            var lessons = events.Select(ev =>
            {
                var description = ev.Description.Split('\n');
                var address = ev.Location.Split('/');
                return new Lesson
                {
                    Address = address[0],
                    Auditory = address[1],
                    Number = int.Parse(description[0][0].ToString()),
                    Groups = description[1].Substring(3),
                    Name = description[2],
                    Type = description[3],
                    Teacher = description[4],
                    StartTime = ev.DtStart.AsSystemLocal,
                    EndTime = ev.DtEnd.AsSystemLocal
                };
            });

            return lessons;
        }
    }
}