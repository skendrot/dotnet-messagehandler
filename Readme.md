#Runscope Message Handlers


##Runscope Proxy Message Handler

Runscope Proxy Message Handler for use with System.Net.Http.HttpClient

- Requires a free Runscope account, [sign up here](https://www.runscope.com/signup)
- Automatically create Runscope URLs for your requests
- Portable library, works with .net 4.5, Windows Phone 8.1, and Windows Store Apps.  

### Installation

    PM> Install-Package Runscope.Contrib -Pre   


### Runscope Proxy Message Handler
This message handle is useful when you are consuming an API.

    using System.Net.Http;
    using Runscope.Contrib;
    
    var runscopeHandler = new RunscopeProxyMessageHandler("bucketKey", new HttpClientHandler());
 
    var httpClient = new HttpClient(runscopeHandler);
 
    var response = await httpClient.GetAsync("https://api.github.com");


##Runscope API Message Handler
This message handler is useful when you are a provider of an API.  It can also be used from the client, but that would expose your API key in the client.

See this (http://bizcoder.com/add-runscope-logging-to-your-asp-net-web-api-in-minutes) blog post for details on how to use the RunscopeAPIMessageHandler.
