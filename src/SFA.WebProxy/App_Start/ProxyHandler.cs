namespace SFA.WebProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Factory;
    using Logging;
    using Models;
    using Routing;
    using System.IO;
    public class ProxyHandler : DelegatingHandler
    {
        private readonly IProxyRouting _proxyRouting;
        private readonly IProxyLogging _proxyLogging;
        private readonly IConfiguration _configuration;

        public ProxyHandler(IProxyRouting proxyRouting, IProxyLogging proxyLogging, IConfiguration configuration)
        {
            _proxyRouting = proxyRouting;
            _proxyLogging = proxyLogging;
            _configuration = configuration;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Note: No guarantee headers will end up being sent in the same order as originally
            // Note: Http protocol version may not be retained
            var requestContent = request.Method == HttpMethod.Get ? null : await request.Content.ReadAsStringAsync();

            //Log request coming in against a new identifier
            var routeIdentifier = new RouteIdentifier();
            _proxyLogging.LogRequest(request, requestContent, routeIdentifier);

            var routing = _proxyRouting.GetRouting(request.RequestUri, request.Method, GetClientIPAddress(request), requestContent, routeIdentifier);

            var responses = new List<Task<HttpResponseMessage>>();

            if (request.Method == HttpMethod.Get)
            {
                foreach (var route in routing.Routes)
                {
                    if (route.IsPrimary)
                    {
                        responses.Insert(0, GetAsyncRequest(request, route, routing));
                    }
                    else if (_configuration.AreNonPrimaryRequestsEnabled)
                    {
                        responses.Add(GetAsyncRequest(request, route, routing));
                    }
                }
            }
            else if (request.Method == HttpMethod.Post)
            {
                foreach (var route in routing.Routes)
                {
                    if (route.IsPrimary)
                    {
                        var requestHttpContent = GetRequestHttpContent(request, requestContent);
                        responses.Insert(0, PostAsyncRequest(request, requestHttpContent, route, routing));
                    }
                    else if (_configuration.AreNonPrimaryRequestsEnabled)
                    {
                        var requestHttpContent = GetRequestHttpContent(request, requestContent);
                        responses.Add(PostAsyncRequest(request, requestHttpContent, route, routing));
                    }
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            //Primary task is always inserted as first object
            return await responses.First();
        }

        private static StringContent GetRequestHttpContent(HttpRequestMessage request, string requestContent)
        {
            var requestHttpContent = new StringContent(requestContent);
            requestHttpContent.Headers.Clear();
            // Copy request content headers (.NET treats these separately from non-content headers)
            foreach (var header in request.Content.Headers)
            {
                requestHttpContent.Headers.Add(header.Key, header.Value);
            }
            return requestHttpContent;
        }

        private Task<HttpResponseMessage> GetAsyncRequest(HttpRequestMessage request, Route route, Models.Routing routing)
        {
            var client = RoutingHttpClientFactory.Create(request, route.Uri);
            return client.GetAsync(route.Uri).ContinueWith(ContinuationFunction(client, null, route, routing));
        }

        private Task<HttpResponseMessage> PostAsyncRequest(HttpRequestMessage request, HttpContent requestHttpContent, Route route, Models.Routing routing)
        {
            var client = RoutingHttpClientFactory.Create(request, route.Uri);
            var contentHeaders = request.Content?.Headers;
            return client.PostAsync(route.Uri, requestHttpContent).ContinueWith(ContinuationFunction(client, contentHeaders, route, routing));
        }

        private Func<Task<HttpResponseMessage>, HttpResponseMessage> ContinuationFunction(HttpClient client, HttpContentHeaders contentHeaders, Route route, Models.Routing routing)
        {
            return task =>
            {
                client.Dispose();

                if (task.IsCanceled)
                {
                    _proxyLogging.LogResponseCancelled(route, client.DefaultRequestHeaders, contentHeaders, task.Exception);
                    if (route.IsPrimary)
                    {
                        //Will display underlying exception
                        return task.Result;
                    }
                    return null;
                }

                if (task.IsFaulted)
                {
                    _proxyLogging.LogResponseFaulted(route, client.DefaultRequestHeaders, contentHeaders, task.Exception);
                    if (route.IsPrimary)
                    {
                        //Will display underlying exception
                        return task.Result;
                    }
                    return null;
                }

                if (task.IsCompleted)
                {
                    var memoryStream = new MemoryStream();
                    var encoding = System.Text.Encoding.UTF8;

                    int replacements = SearchReplace(task.Result.Content.ReadAsStreamAsync().Result, memoryStream,
                        encoding.GetBytes(routing.RewriteFrom.ToLower()), encoding.GetBytes(routing.RewriteTo));

                    _proxyLogging.LogResponseContent(new MemoryStream(memoryStream.GetBuffer()), route.Identifier);

                    if (route.IsPrimary)
                    {
                        var result = new HttpResponseMessage(task.Result.StatusCode);
                        result.Headers.Clear();

                        result.Headers.Add("X-Response-From", route.Uri.ToString());

                        // May be useful for debugging
                        //result.Headers.Add("X-MemoryStream-Length", $"{memoryStream.Length}");
                        result.Headers.Add("X-Replacement", $"{routing.RewriteFrom} to {routing.RewriteTo}: {replacements}");

                        result.Content = new ByteArrayContent(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                        result.Content.Headers.Add("Content-Length", $"{memoryStream.Length}");
                        foreach (var header in task.Result.Content.Headers)
                        {
                            if (!result.Content.Headers.Contains(header.Key))
                                result.Content.Headers.Add(header.Key, header.Value);
                        }

                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }

                throw new Exception("Unhandled");
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="input">Assumed to be UTF-8</param>
        /// <param name="output"></param>
        /// <param name="search">Case insensitive within ASCII range provided the search term is in lower case.</param>
        /// <param name="replace"></param>
        private int SearchReplace(Stream input, Stream output, byte[] search, byte[] replace)
        {
            var matchBuffer = new byte[search.Length];

            int replacements = 0;

            int searchPos = 0;
            while (true)
            {
                int thisByte = input.ReadByte();

                if (thisByte == search[searchPos] || (thisByte >= 'A' && thisByte <= 'Z' && (thisByte - 'A' + 'a') == search[searchPos]))
                {
                    matchBuffer[searchPos++] = (byte)thisByte;

                    if (searchPos == search.Length)
                    {
                        replacements++;
                        output.Write(replace, 0, replace.Length);
                        searchPos = 0;
                    }
                }
                else
                {
                    if (searchPos > 0)
                    {
                        output.Write(matchBuffer, 0, searchPos);
                        searchPos = 0;
                    }

                    if (thisByte == -1)
                        break;

                    output.WriteByte((byte)thisByte);
                }
            }

            return replacements;
        }

        // Credit: http://stackoverflow.com/questions/9565889/get-the-ip-address-of-the-remote-host
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage =
            "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        private const string OwinContext = "MS_OwinContext";

        public static string GetClientIPAddress(HttpRequestMessage request)
        {
            // Web-hosting. Needs reference to System.Web.dll
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            // Self-hosting. Needs reference to System.ServiceModel.dll. 
            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            // Self-hosting using Owin. Needs reference to Microsoft.Owin.dll. 
            if (request.Properties.ContainsKey(OwinContext))
            {
                dynamic owinContext = request.Properties[OwinContext];
                if (owinContext != null)
                {
                    return owinContext.Request.RemoteIpAddress;
                }
            }

            return null;
        }
    }
}