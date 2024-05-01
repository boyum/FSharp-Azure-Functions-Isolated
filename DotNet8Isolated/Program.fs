open Microsoft.Extensions.Hosting
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Extensions.DurableTask

HostBuilder()
    .ConfigureFunctionsWorkerDefaults(fun (applicationBuilder: IFunctionsWorkerApplicationBuilder) ->
        DurableTaskExtensionStartup()
            .Configure(applicationBuilder)
        |> ignore)
    .Build()
    .Run()
