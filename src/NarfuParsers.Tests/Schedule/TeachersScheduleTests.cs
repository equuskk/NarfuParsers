using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task GetSchedule_CorrectId_ReturnsLessons()
        {
            const int teacherId = 22222;
            var correctStartDate = new DateTime(2019, 05, 14, 5, 20, 00);
            var correctEndDate = new DateTime(2019, 05, 14, 6, 55, 00);

            var timeout = TimeSpan.FromSeconds(5);
            var service = new TeachersSchedule(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(await File.ReadAllTextAsync("TestData/Schedule/teachers.txt"));

                var result = await service.GetLessons(teacherId);
                var first = result.First();

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .WithQueryParamValue("lecturer", teacherId)
                        .Times(1);

                result.Should().NotBeEmpty().And
                      .HaveCount(48);
                first.Address.Should().Be("наб. Северной Двины, д. 17");
                first.Auditory.Should().Be("ауд. 1255");
                first.EndTime.ToUniversalTime().Should().Be(correctEndDate);
                first.Groups.Should().Be("Группа \"301713\"");
                first.Name.Should().Be("Физическая химия");
                first.Number.Should().Be(1);
                first.StartTime.ToUniversalTime().Should().Be(correctStartDate);
                first.Teacher.Should().Be("Богданов Михаил Владиславович. Кафедра теоретической и прикладной химии");
                first.Type.Should().Be("Лабораторные занятия");
            }
        }

        [Fact]
        public async Task GetSchedule_IncorrectId_ReturnsLessons()
        {
            const int teacherId = 1;
            var timeout = TimeSpan.FromSeconds(5);
            var service = new TeachersSchedule(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(string.Empty, 404);

                Func<Task> func = async () => await service.GetLessons(teacherId);
                await func.Should().ThrowAsync<FlurlHttpException>();

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .WithQueryParamValue("lecturer", teacherId)
                        .Times(1);
            }
        }
    }
}