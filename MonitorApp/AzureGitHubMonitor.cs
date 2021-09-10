using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MonitorApp
{
    public static class AzureGitHubMonitor
    {
        [Function("AzureGitHubMonitor")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequestData req,
            ILogger log)
        {
            log.LogInformation("Our GitHub monitor processed an action");

            var name = await req.ReadAsStringAsync();
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (data != null) name ??= data.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name");
        }
    }
}