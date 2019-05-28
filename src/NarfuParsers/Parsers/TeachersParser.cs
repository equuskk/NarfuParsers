using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl.Http;
using HtmlAgilityPack;
using NarfuParsers.Common;
using NarfuParsers.Entities;

namespace NarfuParsers.Parsers
{
    public class TeachersParser
    {
        private readonly IFlurlRequest _client;

        public TeachersParser(TimeSpan timeout)
        {
            _client = FlurlClientBuilder.BuildClient(timeout);
        }

        /// <summary>
        ///     Получить перечисление преподавателей в указанном диапазоне
        /// </summary>
        /// <param name="startId">Начальный ID преподавателя</param>
        /// <param name="endId">Конечный ID преподавателя</param>
        /// <param name="sleepEvery">Через сколько запросов выполнять задержку</param>
        /// <param name="sleepMs">Сколько милисекунд задержка</param>
        /// <returns>Перечисление преподавателей в указанном диапазоне</returns>
        public async Task<IEnumerable<Teacher>> GetTeachersInRange(int startId, int endId,
                                                                   int sleepEvery, int sleepMs)
        {
            var teachers = new List<Teacher>();
            var c = 0;
            for(var i = startId; i < endId; i++)
            {
                var response = await _client
                                     .SetQueryParam("timetable")
                                     .SetQueryParam("lecturer", i)
                                     .AllowAnyHttpStatus()
                                     .GetAsync();
                if(!response.IsSuccessStatusCode)
                {
                    continue;
                }

                var doc = new HtmlDocument();
                doc.Load(await response.Content.ReadAsStreamAsync());

                var teacher = doc.DocumentNode.SelectSingleNode("//a[@class='navbar-brand']/span[2]").InnerText.Trim();

                if(string.IsNullOrEmpty(teacher))
                {
                    continue; // значит такой преподаватель удалён
                }

                var teacherSplit = Regex.Replace(teacher, @"(\s{2,})", "").Split('.');

                teachers.Add(new Teacher
                {
                    Id = i,
                    Name = teacherSplit[0],
                    Depart = teacherSplit[1]
                });

                if(c++ % sleepEvery == 0)
                {
                    await Task.Delay(sleepMs);
                }
            }

            return teachers;
        }
    }
}