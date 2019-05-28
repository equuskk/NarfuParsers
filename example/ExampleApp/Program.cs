﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NarfuParsers.Parsers;
using NarfuParsers.Schedule;
using NarfuParsers.Common;

namespace ExampleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var teacherSchedule = new TeachersSchedule();
            var teacherLessons = await teacherSchedule.GetLessons(22914);

            var studentsSchedule = new StudentsSchedule();
            var studentsLessons = await studentsSchedule.GetLessons(9092);

            var schoolsParser = new SchoolsParser();
            var schools = await schoolsParser.GetSchools();

            var groupsParser = new GroupsParser();
            var groups = await groupsParser.GetGroupsFromSchool(15);

            var httpHandler = new HttpClientHandler
            {
                Proxy = new WebProxy(new Uri("http://***.***.***.***:****"))
            };
            var httpClient = new HttpClient(httpHandler)
            {
                BaseAddress = new Uri(Constants.EndPoint)
            };

            var teachersParser = new TeachersParser(httpClient);
            var teachers = await teachersParser.GetTeachersInRange(22913, 22917, 3, 1000);

            Console.ReadLine();
        }
    }
}
