using MyTrap.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTrap.Api.Handlers
{
    public abstract class MessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Stopwatch timeRequest = new Stopwatch();

            timeRequest.Start();

            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            var requestInfo = string.Format("{0} {1}", request.Method, request.RequestUri.LocalPath);

            IEnumerable<string> authorizations = new List<string>();

            bool isAuthorization = request.Headers.TryGetValues("Authorization", out authorizations);

            var authorization = isAuthorization ? authorizations.FirstOrDefault() : "";

            var requestMessage = await request.Content.ReadAsByteArrayAsync();

            await IncommingMessageAsync(corrId, requestInfo, requestMessage, authorization);

            var response = await base.SendAsync(request, cancellationToken);

            byte[] responseMessage;

            if (response.IsSuccessStatusCode)
            {
                responseMessage = await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
            }

            await OutgoingMessageAsync(corrId, requestInfo, responseMessage, timeRequest);

            return response;
        }

        protected abstract Task IncommingMessageAsync(string correlationId, string requestInfo, byte[] message, string authorization);
        protected abstract Task OutgoingMessageAsync(string correlationId, string requestInfo, byte[] message, Stopwatch timeRequest);
    }

    public class MessageLoggingHandler : MessageHandler
    {
        protected override async Task IncommingMessageAsync(string correlationId, string requestInfo, byte[] message, string authorization)
        {
            await Task.Run(() =>
            {
                //Debug.WriteLine(string.Format("{0} - Request: {1}\r\n{2}", correlationId, requestInfo, Encoding.UTF8.GetString(message)));
                //Debug.WriteLine(string.Format("Token {0}", authorization));

                ElmahUtils.LogSuccessToElmah(string.Format("{0} - Request: {1}\r\n{2}\r\n{3}", correlationId, requestInfo, Encoding.UTF8.GetString(message), authorization));
            });
        }

        protected override async Task OutgoingMessageAsync(string correlationId, string requestInfo, byte[] message, Stopwatch timeRequest)
        {
            await Task.Run(() =>
            {
                //Debug.WriteLine(string.Format("{0} - Response (ms):{1} - {2}\r\n{3}", correlationId, timeRequest.ElapsedMilliseconds, requestInfo, Encoding.UTF8.GetString(message)));

                ElmahUtils.LogSuccessToElmah(string.Format("{0} - Response (ms):{1} - {2}\r\n{3}", correlationId, timeRequest.ElapsedMilliseconds, requestInfo, Encoding.UTF8.GetString(message)));
            });
        }
    }
}