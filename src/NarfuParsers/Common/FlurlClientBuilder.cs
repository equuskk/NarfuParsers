using System;
using Flurl.Http;

namespace NarfuParsers.Common
{
    public class FlurlClientBuilder
    {
        /// <summary>
        ///     Создает HttpClient с заданным timeout'ом и user agent'ом
        /// </summary>
        /// <param name="timeout">Сколько ждать загрузки страницы</param>
        /// <param name="userAgent">User agent</param>
        /// <returns>Сконфигурированный FlurlClient для запросов к сайту</returns>
        public static IFlurlRequest BuildClient(TimeSpan timeout, string userAgent = null)
        {
            return Constants.EndPoint.WithTimeout(timeout)
                            .WithHeaders(new
                            {
                                User_Agent = userAgent ?? Constants.DefaultUserAgent
                            });
        }
    }
}