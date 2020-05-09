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
    public class GroupsParserTests
    {
        [Fact]
        public async Task GetGroupsFromSchool_CorrectSchoolId_ReturnsGroups()
        {
            const int schoolId = 3;
            var timeout = TimeSpan.FromSeconds(5);
            var service = new GroupsParser(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(await File.ReadAllTextAsync("TestData/Parsers/groups.html"));

                var result = await service.GetGroupsFromSchool(schoolId);
                var first = result.First();

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .WithQueryParam("groups")
                        .WithQueryParamValue("institution", schoolId)
                        .Times(1);

                result.Should().NotBeEmpty().And
                      .HaveCount(13);
                first.Name.Should()
                     .Be("Автоматизация технологических процессов и производств (Автоматизация систем управления производством)");
                first.RealId.Should().Be(351810);
                first.SiteId.Should().Be(9584);
            }
        }

        [Fact]
        public async Task GetGroupsFromSchool_IncorrectSchoolId_ThrowsException()
        {
            const int schoolId = 9999;
            var timeout = TimeSpan.FromSeconds(5);
            var service = new GroupsParser(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(string.Empty, 404);

                Func<Task> func = async () => await service.GetGroupsFromSchool(schoolId);

                await func.Should().ThrowAsync<FlurlHttpException>();

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .WithQueryParam("groups")
                        .WithQueryParamValue("institution", schoolId)
                        .Times(1);
            }
        }
    }
}