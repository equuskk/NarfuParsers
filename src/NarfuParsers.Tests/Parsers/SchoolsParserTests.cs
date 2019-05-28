using System;
using System.IO;
using System.Net.Http;
using Flurl.Http.Testing;
using NarfuParsers.Common;
using NarfuParsers.Parsers;
using Xunit;

namespace NarfuParsers.Tests.Parsers
{
    public class SchoolsParserTests
    {
        [Fact]
        public async void GetSchools_CorrectData_Schools()
        {
            var timeout = TimeSpan.FromSeconds(5);
            var service = new SchoolsParser(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(File.ReadAllText("TestData/Parsers/schools.html"));

                var result = await service.GetSchools();

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .Times(1);

                Assert.NotEmpty(result);
            }
        }
    }
}