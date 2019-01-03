
Tanka GraphQL .NET Client library
=====================================

This library provides easy to use cross-platform client API for [Tanka GraphQL execution library](https://github.com/pekkah/tanka-graphql) based server using [ASP.NET Core SignalR .NET Client](https://docs.microsoft.com/en-us/aspnet/core/signalr/dotnet-client?view=aspnetcore-2.2)s `HubConnection`. 

## Features

- Execute queries, mutations and subsriptions using SignalR HubConnection
- Supports GraphQL validation and tracing
- Leverage power of SignalR communication techniques (WebSockets, Server-Sent Events, Long Polling) 

## Gettings started

### Add usings

```csharp
using Tanka.GraphQL;
```

### Defining models

You can define your models as POCOs. In these examples, I'm using separate models for input and output messages. 

```csharp
public class Message
{
    public int Id { get; set; }
    public string Content { get; set; }
}

public class InputMessage
{
    public string Content { get; set; }
}

```

### Connect to the server

Create SignalR `HubConnection` normally. Read more [how to connect to a hub](https://docs.microsoft.com/en-us/aspnet/core/signalr/dotnet-client?view=aspnetcore-2.2#connect-to-a-hub)

```csharp
var connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/hubs/graphql")
                    .Build();
await connection.StartAsync();
```

### Query

```csharp
var channelId = 1;

var channelMessageGQL = @"query Messages($channelId: Int!) {
                messages(channelId: $channelId) {
                  id
                  content
                }
            }";

var queryRequest = new QueryRequest()
{
    Query = channelMessageGQL,
    Variables = new Dictionary<string, object>()
        {
            { "channelId", channelId }
        }
};

var result = await connection.QueryAsync(queryRequest);
var data = result.GetDataFieldAs<List<Messages>>();
````

### Mutation

In GraphQL both queries and mutations are defined as **queries**. 

```csharp
var postMessageMutationGQL = @"mutation PostMessage($channelId: Int!, $message: InputMessage) {
               postMessage(channelId: $channelId, message: $message) {
                 id
                 content
               }
            }";

var queryRequest = new QueryRequest()
{
    Query = postMessageMutationGQL,
    Variables = new Dictionary<string, object>()
        {
            { "channelId", channelId },
            { "message", new InputMessage() { Content = message } }
        }
};

var result = await connection.QueryAsync(queryRequest);
var data = result.GetDataFieldAs<Messages>();
```

### Subscription

API provides support for subscriptions as streams using `IObservable<ExecutionResult>`. You can subscripe to the stream using `Subscribe` method.

```csharp
 var channelSubsribtionGQL = @"subscription MessageAdded($channelId: Int!) {
                messageAdded(channelId: $channelId) {
                  id
                  content
                }
            }";

var queryRequest = new QueryRequest()
{
    Query = channelSubsribtionGQL,
    Variables = new Dictionary<string, object>()
        {
            { "channelId", channelId}
        }
};

var serverSubscription = await connection.SubscribeAsync(request);
serverSubscription.Subscribe(
                // On new message added
                result =>
                {
                    var message = result.GetDataFieldAs<Message>();
                    // Handle new message
                },
                // On error corrured
                error =>
                {
                    //Handle error
                },
                // On completed
                () =>
                {
                   //No more messages coming
                });
```
