using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
using NarfuParsers.Common;
using NarfuParsers.Parsers;
using Xunit;

namespace NarfuParsers.Tests.Parsers
{
    public class TeachersParserTests
    {
        [Fact]
        public async Task GetTeachersInRange_CorrectData_Teachers()
        {
            const int startId = 22000;
            const int endId = 22100;
            const int diff = endId - startId;

            var timeout = TimeSpan.FromSeconds(5);
            var service = new TeachersParser(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(await File.ReadAllTextAsync("TestData/Schedule/teachers.txt"));

                var result = await service.GetTeachersInRange(startId, endId, 1, 1);
                var first = result.First();

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .Times(diff);

                result.Should().NotBeEmpty().And
                      .HaveCount(1);
                first.Depart.Should().Be("Кафедра теоретической и прикладной химии");
                first.Id.Should().Be(22000);
                first.Name.Should().Be("Богданов Михаил Владиславович");
            }
        }
    }
}