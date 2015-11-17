namespace SFA.WebProxy
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public class ProxyHandler : DelegatingHandler
    {
        private readonly IProxyRouting _proxyRouting;

        public ProxyHandler(IProxyRouting proxyRouting)
        {
            _proxyRouting = proxyRouting;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Note: Redirects are handled on the proxy side and therefore the client doesn't see them. This can result in resources not being found.
            // Note: No guarantee headers will end up being sent in the same order as originally
            // Note: Http protocol version may not be retained

            var requestContent = (request.Method == HttpMethod.Get) ? null : request.Content.ReadAsStringAsync();

            var routing = _proxyRouting.GetRouting(request.RequestUri, request.Method, GetClientIPAddress(request), requestContent);

            var responses = new List<Task<HttpResponseMessage>>();

            using (var client = new HttpClient())
            {

                // Copy request headers (.NET treats request content headers separately)
                client.DefaultRequestHeaders.Clear();
                foreach (var header in request.Headers)
                {
                    if (header.Key == "Host")
                        client.DefaultRequestHeaders.Add(header.Key, routing.PrimaryUri.Host); // TODO: Ideally would vary
                    else
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                if (request.Method == HttpMethod.Get)
                {
                    responses.Add(client.GetAsync(routing.PrimaryUri));
                    foreach (var secondaryUri in routing.SecondaryUris)
                    {
                        responses.Add(client.GetAsync(secondaryUri));
                    }
                }
                else if (request.Method == HttpMethod.Post)
                {
                    var requestContentAsString = await requestContent;
                    {
                        // Copy request content headers (.NET treats these separately from non-content headers)
                        var requestHttpContent = new StringContent(requestContentAsString);
                        requestHttpContent.Headers.Clear();
                        foreach (var header in request.Content.Headers)
                        {
                            requestHttpContent.Headers.Add(header.Key, header.Value);
                        }

                        responses.Add(client.PostAsync(routing.PrimaryUri, requestHttpContent));
                    }

                    foreach (var secondaryUri in routing.SecondaryUris)
                    {
                        var requestHttpContent = new StringContent(requestContentAsString);
                        requestHttpContent.Headers.Clear();
                        foreach (var header in request.Content.Headers)
                        {
                            requestHttpContent.Headers.Add(header.Key, header.Value);
                        }

                        responses.Add(client.PostAsync(routing.PrimaryUri, requestHttpContent));
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }

                return await responses[0].ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        throw new Exception("Request to " + routing.PrimaryUri + " with headers " + string.Join(", ", client.DefaultRequestHeaders.Select(h => h.Key + ":" + string.Join("|", h.Value))), task.Exception);
                    }

                    if (task.IsCompleted)
                    {
                        try
                        {
                            task.Result.Headers.Add("X-Response-From", routing.PrimaryUri.ToString());

                            var httpContent = (StreamContent)task.Result.Content;
                            var property = httpContent.GetType().GetField("content", BindingFlags.Instance | BindingFlags.NonPublic);
                            if (property == null)
                                throw new Exception("Can't find content property");
                            var oldStream = (Stream)property.GetValue(httpContent);
                            property.SetValue(httpContent, new DelegatingStream(oldStream));

                        }
                        catch (Exception ex)
                        {
                            // TODO: Log reason for not having a copy of the result
                        }
                    }

                    return task.Result;
                }
            );
            }
        }

        public static void SetProperty(object instance, string propertyName, object newValue)
        {
            var type = instance.GetType();

            var prop = type.BaseType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);

            prop.SetValue(instance, newValue, null);
        }


        // Copied from System.Net.Http
        internal class DelegatingStream : Stream
        {
            private readonly Stream _innerStream;

            public override bool CanRead
            {
                get
                {
                    return _innerStream.CanRead;
                }
            }

            public override bool CanSeek
            {
                get
                {
                    return _innerStream.CanSeek;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    return _innerStream.CanWrite;
                }
            }

            public override long Length
            {
                get
                {
                    return _innerStream.Length;
                }
            }

            public override long Position
            {
                get
                {
                    return _innerStream.Position;
                }
                set
                {
                    _innerStream.Position = value;
                }
            }

            public override int ReadTimeout
            {
                get
                {
                    return _innerStream.ReadTimeout;
                }
                set
                {
                    _innerStream.ReadTimeout = value;
                }
            }

            public override bool CanTimeout
            {
                get
                {
                    return _innerStream.CanTimeout;
                }
            }

            public override int WriteTimeout
            {
                get
                {
                    return _innerStream.WriteTimeout;
                }
                set
                {
                    _innerStream.WriteTimeout = value;
                }
            }

            public DelegatingStream(Stream innerStream)
            {
                _innerStream = innerStream;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                    _innerStream.Dispose();
                base.Dispose(disposing);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return _innerStream.Seek(offset, origin);
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _innerStream.Read(buffer, offset, count);
            }

            public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                return _innerStream.BeginRead(buffer, offset, count, callback, state);
            }

            public override int EndRead(IAsyncResult asyncResult)
            {
                return _innerStream.EndRead(asyncResult);
            }

            public override int ReadByte()
            {
                return _innerStream.ReadByte();
            }

            public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                return _innerStream.ReadAsync(buffer, offset, count, cancellationToken);
            }

            public override void Flush()
            {
                _innerStream.Flush();
            }

            public override Task FlushAsync(CancellationToken cancellationToken)
            {
                return _innerStream.FlushAsync(cancellationToken);
            }

            public override void SetLength(long value)
            {
                _innerStream.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                _innerStream.Write(buffer, offset, count);
            }

            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                return _innerStream.BeginWrite(buffer, offset, count, callback, state);
            }

            public override void EndWrite(IAsyncResult asyncResult)
            {
                _innerStream.EndWrite(asyncResult);
            }

            public override void WriteByte(byte value)
            {
                _innerStream.WriteByte(value);
            }

            public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                return _innerStream.WriteAsync(buffer, offset, count, cancellationToken);
            }
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