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
    public class StudentsScheduleTests
    {
        [Fact]
        public async Task GetSchedule_CorrectGroupId_ReturnsLessons()
        {
            const int group = 9092;
            var date = new DateTime(2019, 05, 28);
            var correctStartDate = new DateTime(2019, 05, 14, 7, 10, 00);
            var correctEndDate = new DateTime(2019, 05, 14, 8, 45, 00);

            var timeout = TimeSpan.FromSeconds(5);
            var service = new StudentsSchedule(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(await File.ReadAllTextAsync("TestData/Schedule/students.txt"));

                var result = await service.GetLessons(group, date);
                var first = result.First();

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .WithQueryParamValue("oid", group)
                        .WithQueryParamValue("from", date.ToString("dd.MM.yyyy"))
                        .Times(1);

                result.Should().NotBeEmpty().And
                      .HaveCount(4);
                first.Address.Should().Be("А-НСД2к2");
                first.Auditory.Should().Be("10101");
                first.EndTime.ToUniversalTime().Should().Be(correctEndDate);
                first.Groups.Should().Be("555555");
                first.Name.Should().Be("Предмет2");
                first.Number.Should().Be(2);
                first.StartTime.ToUniversalTime().Should().Be(correctStartDate);
                first.Teacher.Should().Be("Препод2");
                first.Type.Should().Be("Лабораторные занятия");
            }
        }

        [Fact]
        public async Task GetSchedule_IncorrectGroupId_ThrowsException()
        {
            const int group = 999999;
            var date = new DateTime(2019, 05, 28);
            var timeout = TimeSpan.FromSeconds(5);
            var service = new StudentsSchedule(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(string.Empty, 404);

                Func<Task> func = async () => await service.GetLessons(group, date);

                await func.Should().ThrowAsync<FlurlHttpException>();

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .WithQueryParamValue("oid", group)
                        .WithQueryParamValue("from", date.ToString("dd.MM.yyyy"))
                        .Times(1);
            }
        }
    }
}