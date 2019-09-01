using System;
using System.IO;
using System.Net.Http;
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
        public async void GetGroupsFromSchool_CorrectSchoolId_ReturnsGroups()
        {
            const int schoolId = 3;
            var timeout = TimeSpan.FromSeconds(5);
            var service = new GroupsParser(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(File.ReadAllText("TestData/Parsers/groups.html"));

                var result = await service.GetGroupsFromSchool(schoolId);

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .WithQueryParam("groups")
                        .WithQueryParamValue("institution", schoolId)
                        .Times(1);

                Assert.NotEmpty(result);
            }
        }
        
        [Fact]
        public async void GetGroupsFromSchool_IncorrectSchoolId_ThrowsException()
        {
            const int schoolId = 9999;
            var timeout = TimeSpan.FromSeconds(5);
            var service = new GroupsParser(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith("", 404);

                await Assert.ThrowsAsync<FlurlHttpException>(async () =>
                                                                     await service.GetGroupsFromSchool(schoolId));

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .WithQueryParam("groups")
                        .WithQueryParamValue("institution", schoolId)
                        .Times(1);
            }
        }
    }
}