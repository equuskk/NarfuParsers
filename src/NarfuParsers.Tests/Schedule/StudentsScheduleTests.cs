﻿using System;
using System.IO;
using System.Net.Http;
using Flurl.Http.Testing;
using NarfuParsers.Common;
using NarfuParsers.Schedule;
using Xunit;

namespace NarfuParsers.Tests.Schedule
{
    public class StudentsScheduleTests
    {
        [Fact]
        public async void GetSchedule_CorrectParams_Lessons()
        {
            const int group = 9092;
            var date = new DateTime(2019, 05, 28);
            var timeout = TimeSpan.FromSeconds(5);
            var service = new StudentsSchedule(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(File.ReadAllText("TestData/Schedule/students.txt"));

                var result = await service.GetLessons(group, date);

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .WithQueryParamValue("oid", group)
                        .WithQueryParamValue("from", date.ToString("dd.MM.yyyy"))
                        .Times(1);

                Assert.NotEmpty(result);
            }
        }
    }
}