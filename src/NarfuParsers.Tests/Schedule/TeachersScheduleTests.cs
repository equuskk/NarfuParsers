using System;
using System.IO;
using System.Net.Http;
using Flurl.Http;
using Flurl.Http.Testing;
using NarfuParsers.Common;
using NarfuParsers.Schedule;
using Xunit;

namespace NarfuParsers.Tests.Schedule
{
    public class TeachersScheduleTests
    {
        [Fact]
        public async void GetSchedule_CorrectId_ReturnsLessons()
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
        
        [Fact]
        public async void GetSchedule_IncorrectId_ReturnsLessons()
        {
            const int teacherId = 1;
            var timeout = TimeSpan.FromSeconds(5);
            var service = new TeachersSchedule(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith("", 404);

                await Assert.ThrowsAsync<FlurlHttpException>(async () => await service.GetLessons(teacherId));
                
                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .WithQueryParamValue("lecturer", teacherId)
                        .Times(1);
            }
        }
    }
}