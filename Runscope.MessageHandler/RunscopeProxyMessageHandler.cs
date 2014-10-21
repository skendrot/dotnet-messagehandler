using System;
using System.Net.Http;

namespace Runscope
{
    public class RunscopeProxyMessageHandler : DelegatingHandler
    {
        private readonly string _bucketKey;
        
        public RunscopeProxyMessageHandler(string bucketKey, HttpMessageHandler innerHandler)
        {
            _bucketKey = bucketKey;
            InnerHandler = innerHandler;
        }

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var requestUri = request.RequestUri;
            var port = requestUri.Port;

            request.RequestUri = requestUri.ToRunscopeUrl(_bucketKey);
            if ((requestUri.Scheme == "http" && port != 80 )||  requestUri.Scheme == "https" && port != 443)
            {
                request.Headers.TryAddWithoutValidation("Runscope-Request-Port", port.ToString());
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
