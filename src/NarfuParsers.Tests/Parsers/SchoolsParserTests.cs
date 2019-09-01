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
    public class SchoolsParserTests
    {
        [Fact]
        public async void GetSchools_SiteIsAvailable_ReturnsSchools()
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
        
        [Fact]
        public async void GetSchools_SiteIsNotAvailable_ThrowsException()
        {
            var timeout = TimeSpan.FromSeconds(5);
            var service = new SchoolsParser(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith("", 404);

                await Assert.ThrowsAsync<FlurlHttpException>(async () => await service.GetSchools());

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .Times(1);
            }
        }
    }
}