namespace My.Function

open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.DurableTask
open System.Net
open System.Collections.Generic

type Execute(logger: ILogger<Execute>) =

    [<Function("HttpTrigger")>]
    member _.Run
        (
            [<HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "hello")>] req: HttpRequestData,
            [<DurableClient>] starter: Client.DurableTaskClient,
            executionContext: FunctionContext
        ) =
        task {
            logger.LogInformation($"Hello at {System.DateTime.UtcNow} from an Azure function using F# on .NET 8.")

            let orchestrationOutput =
                starter.ScheduleNewOrchestrationInstanceAsync("OrchestrationTrigger")
                |> Async.AwaitTask
                |> Async.RunSynchronously


            let response = req.CreateResponse(HttpStatusCode.OK)
            do! response.WriteStringAsync($"Hello World \n {orchestrationOutput}")
            return response
        }

    [<Function("OrchestrationTrigger")>]
    member _.RunOrchestration([<OrchestrationTrigger>] context: TaskOrchestrationContext) =
        async {
            let outputs =
                [| context.CallActivityAsync<string>("ActivityTrigger", "Tokyo")
                   |> Async.AwaitTask
                   |> Async.RunSynchronously

                   context.CallActivityAsync<string>("ActivityTrigger", "Seattle")
                   |> Async.AwaitTask
                   |> Async.RunSynchronously

                   context.CallActivityAsync<string>("ActivityTrigger", "London")
                   |> Async.AwaitTask
                   |> Async.RunSynchronously |]

            return outputs |> String.concat ", "
        }
        |> Async.StartAsTask

    [<Function("ActivityTrigger")>]
    member _.Activity([<ActivityTrigger>] name: string, log: ILogger) =
        log.LogInformation($"Saying hello to {name}.")
        $"Hello {name}!"
