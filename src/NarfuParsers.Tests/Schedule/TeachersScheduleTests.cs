using System;
using System.IO;
using System.Net.Http;
using Flurl.Http.Testing;
using NarfuParsers.Common;
using NarfuParsers.Schedule;
using Xunit;

namespace NarfuParsers.Tests.Schedule
{
    public class TeachersScheduleTests
    {
        [Fact]
        public async void GetSchedule_CorrectId_Lessons()
        {
            const int teacherId = 22222;
            var timeout = TimeSpan.FromSeconds(5);
            var service = new TeachersSchedule(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(File.ReadAllText("TestData/Schedule/teachers.txt"));

                var result = await service.GetLessons(teacherId);

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .WithQueryParamValue("lecturer", teacherId)
                        .Times(1);

                Assert.NotEmpty(result);
            }
        }
    }
}