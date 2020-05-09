using System;
using Flurl.Http;

namespace NarfuParsers.Common
{
    public static class FlurlClientBuilder
    {
        /// <summary>
        ///     Создает IFlurlRequest с заданным timeout'ом и user agent'ом
        /// </summary>
        /// <param name="timeout">Сколько ждать загрузки страницы</param>
        /// <param name="userAgent">User agent</param>
        /// <returns>Сконфигурированный IFlurlRequest</returns>
        public static IFlurlRequest BuildClient(TimeSpan timeout, string userAgent = null)
        {
            return Constants.EndPoint
                            .WithTimeout(timeout)
                            .WithHeaders(new
                            {
                                User_Agent = userAgent ?? Constants.DefaultUserAgent
                            });
        }
    }
}