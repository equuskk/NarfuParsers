using System;
using System.IO;
using System.Net.Http;
using Flurl.Http.Testing;
using NarfuParsers.Common;
using NarfuParsers.Parsers;
using Xunit;

namespace NarfuParsers.Tests.Parsers
{
    public class GroupsParserTests
    {
        [Fact]
        public async void GetGroupsFromSchool_CorrectData_Groups()
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
    }
}