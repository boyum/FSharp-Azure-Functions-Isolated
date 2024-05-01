# Example Azure Function using F# on .NET 8

This repo shows a minimal example of how to write an Azure function using F# and run it on .NET 8 isolated mode.

It should be as simple as cloning the repo and running the function app.

Built using Microsoft Visual Studio Community 2022 (64-bit) Version 17.9 and Dotnet 8 version 8.0.204.

⚠️ This example uses Orchestration triggers and needs Azurite to run locally.

### GOTCHAS

I ran into a couple of issues when trying to publish to app to azure:
1. When publishing to azure I could not get linux to work. However windows deploy seems to work fine. Seems linked to this issue [Issue 556](https://github.com/Azure/azure-functions-dotnet-worker/issues/556). I tried all workarounds suggested but alas nothing seemed to work.
2. When publishing I had success when configuring Deployment mode:Self Contained and Target Runtime:Portable.

