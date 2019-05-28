using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Flurl.Http.Testing;
using NarfuParsers.Common;
using NarfuParsers.Parsers;
using Xunit;

namespace NarfuParsers.Tests.Parsers
{
    public class TeachersParserTests
    {
        [Fact]
        public async void GetTeachersInRange_CorrectData_Teachers()
        {
            const int startId = 22000;
            const int endId = 22100;
            const int diff = endId - startId;

            var timeout = TimeSpan.FromSeconds(5);
            var service = new TeachersParser(timeout);

            using(var httpTest = new HttpTest())
            {
                httpTest.RespondWith(File.ReadAllText("TestData/Schedule/teachers.txt"));

                var result = await service.GetTeachersInRange(startId, endId, 1, 1);

                httpTest.ShouldHaveCalled(Constants.EndPoint)
                        .WithVerb(HttpMethod.Get)
                        .Times(diff);

                Assert.NotEmpty(result);
            }
        }
    }
}