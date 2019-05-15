using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ical.Net;
using Narfu.Common;
using Narfu.Domain.Entities;

namespace Narfu.Schedule
{
    public class StudentsSchedule
    {
        private readonly HttpClient _client;

        public StudentsSchedule(HttpClient client = null)
        {
            _client = client ?? HttpClientBuilder.BuildClient(new TimeSpan(0, 0, 5));
        }

        public async Task<IEnumerable<Lesson>> GetLessons(int siteGroupId, DateTime from = default)
        {
            if(from == default(DateTime))
            {
                from = DateTime.Today;
            }

            var response = await _client.GetAsync($"/?icalendar&oid={siteGroupId}&from={from:dd.MM.yyyy}");
            response.EnsureSuccessStatusCode();

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
                    Number = (byte)description[0].ElementAt(0),
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