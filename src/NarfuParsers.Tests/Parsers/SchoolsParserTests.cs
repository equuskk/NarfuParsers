using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Flurl.Http.Testing;
using NarfuParsers.Common;
using NarfuParsers.Parsers;
using Xunit;

namespace NarfuParsers.Tests.Parsers
{
    public class SchoolsParserTests
    {
        [Fact]
        public async Task GetSchools_SiteIsAvailable_ReturnsSchools()
        {
            var timeout = TimeSpan.FromSeconds(5);
            var service = new SchoolsParser(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(await File.ReadAllTextAsync("TestData/Parsers/schools.html"));

                var result = await service.GetSchools();
                var first = result.First();

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                result.Should().NotBeEmpty().And
                      .HaveCount(13);
                first.Id.Should().Be(15);
                first.Name.Should().Be("Высшая инженерная школа");
                first.Url.Should().Be("https://ruz.narfu.ru/?groups&institution=15");
            }
        }

        [Fact]
        public async Task GetSchools_SiteIsNotAvailable_ThrowsException()
        {
            var timeout = TimeSpan.FromSeconds(5);
            var service = new SchoolsParser(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(string.Empty, 404);

                Func<Task> func = async () => await service.GetSchools();

                await func.Should().ThrowAsync<FlurlHttpException>();

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .Times(1);
            }
        }
    }
}