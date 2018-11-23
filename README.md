# BuildMonitor (Work in Progress!)

> Dashboard to display the status of your builds from your build server (e.g. TeamCity).

[![Build Status](https://dev.azure.com/balassy/BuildMonitor/_apis/build/status/BuildMonitor%20pipeline)](https://dev.azure.com/balassy/BuildMonitor/_build/latest?definitionId=1)

## Installation

Set the following **environment variables** to specify the connection to your build server:
- `BUILDMONITOR__HOST`, e.g. `server.example.com:8080`
- `BUILDMONITOR__USERNAME`, e.g. `myuser`
- `BUILDMONITOR__PASSWORD`, e.g. `mypassword`

## Architecture

This solution follows the **Clean Architecture** recommended for ASP.NET Core applications. Read more:

- [Steve Smith: Architect Modern Web Applications with ASP.NET Core and Azure - free eBook](https://docs.microsoft.com/en-us/dotnet/standard/modern-web-apps-azure-architecture/)
- [Jason Taylor: Clean Architecture with ASP.NET Core 2.1 - conference presentation](https://www.youtube.com/watch?v=_lwCVE_XgqI)

![Clean Architecture](./docs/clean-architecture.png "Clean Architecture")

## Got feedback?

Your feedback is more than welcome, please send your suggestions, feature requests or bug reports as [Github issues](https://github.com/balassy/BuildMonitor/issues).

## Contributing guidelines

Contributions of all kinds are welcome, please feel free to send Pull Requests. As they are requirements of successful build all code analyzers and tests MUST pass, and also please make sure you have a reasonable code coverage for new code.

Thanks for your help in making this project better!

## Acknowledgements

Thanks to [Jason Taylor](https://www.youtube.com/watch?v=_lwCVE_XgqI) for the Clean Architecture diagram.

## About the author

This project is created and maintaned by [György Balássy](https://linkedin.com/in/balassy).