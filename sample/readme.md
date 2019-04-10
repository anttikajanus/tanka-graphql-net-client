## Sample chat application using Tanka GraphQL 

A simple chat application using [Tanka GraphQL execution library](https://github.com/pekkah/tanka-graphql) based server using [ASP.NET Core SignalR .NET Client](https://docs.microsoft.com/en-us/aspnet/core/signalr/dotnet-client?view=aspnetcore-2.2). The sample solution (this repository) uses server from [https://github.com/pekkah/tanka-graphql-samples](https://github.com/pekkah/tanka-graphql-samples) repository and demonstrates how to use .NET client library to work with it. Currently the sample contains a WPF chat application and console application which logs the messages sent from the clients.   

The WPF client uses MVVM pattern to separate the logic from the UI and the core of the application is implemented as a .NET Standard 2.0 library. The console application is a Core 2.2 console application using the same shared core.  
