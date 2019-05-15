using System;
using System.Net.Http;

namespace Narfu.Common
{
    public class HttpClientBuilder
    {
        public static HttpClient BuildClient(TimeSpan timeout, string userAgent = null)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(Constants.EndPoint),
                Timeout = timeout
            };

            if(userAgent != null && !string.IsNullOrWhiteSpace(userAgent))
            {
                if(!client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent))
                {
                    throw new ArgumentException("Invalid useragent", nameof(userAgent));
                }
            }

            return client;
        }
    }
}
