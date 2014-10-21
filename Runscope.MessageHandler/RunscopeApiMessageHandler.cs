﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Runscope.Links;
using Runscope.Messages;
namespace Runscope
{
    public class RunscopeMessageHandler : DelegatingHandler
    {
        private readonly string _bucketKey;
        private readonly Func<HttpRequestMessage, HttpResponseMessage, bool> _filter;
        private readonly HttpClient _RunscopeClient;

        public RunscopeMessageHandler(string authtoken, 
                                      string bucketKey,
                                      string runscopeApi = "https://api.runscope.com",
                                      Func<HttpRequestMessage, HttpResponseMessage,bool> filter = null
                                       )
        {
            _bucketKey = bucketKey;
            _filter = filter;
            _RunscopeClient = new HttpClient()
            {
                BaseAddress = new Uri(runscopeApi)
            };
            _RunscopeClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authtoken);
            
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            
            var runscopeRequest = new RunscopeRequest(request);  // Preread to ensure non-rewindable stream is not consumed by app

            var response = await base.SendAsync(request, cancellationToken);

            if (_filter == null || _filter(request, response))
            {
                
                var runscopeMessage2 = new RunscopeMessage()
                {
                    Request = runscopeRequest,
                    Response = new RunscopeResponse(response)
                };

                var messagesLink = new MessagesLink();
                HandleFailedMessageLog(
                    _RunscopeClient.SendAsync(messagesLink.Update(_bucketKey, runscopeMessage2).CreateRequest()));
            }
            return response;
        }

        private void HandleFailedMessageLog(Task<HttpResponseMessage> task)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Debug.WriteLine(task.Exception.Message);
                }

            });
        }
    }
}