using System.Net;
using System.Net.Http;
using Flurl.Http.Configuration;

namespace ExampleAppWithProxy
{
    public class ProxyHttpClientFactory : DefaultHttpClientFactory
    {
        private readonly string _address;

        public ProxyHttpClientFactory(string address)
        {
            _address = address;
        }

        public override HttpMessageHandler CreateMessageHandler()
        {
            return new HttpClientHandler
            {
                Proxy = new WebProxy(_address),
                UseProxy = true
            };
        }
    }
}