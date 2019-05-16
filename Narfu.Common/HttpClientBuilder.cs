using System;
using System.Net.Http;

namespace Narfu.Common
{
    public class HttpClientBuilder
    {
        /// <summary>
        /// Создает HttpClient с заданным timeout'ом и user agent'ом
        /// </summary>
        /// <param name="timeout">Сколько ждать загрузки страницы</param>
        /// <param name="userAgent">User agent</param>
        /// <returns>Сконфигурированный HttpClient для запросов к сайту</returns>
        /// <exception cref="ArgumentException">Был передан некорретный user agent</exception>
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
