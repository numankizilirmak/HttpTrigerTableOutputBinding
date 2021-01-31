using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;

namespace HttpTrigerTableOutputBinding
{
    public static class TableOutputFunction
    {
        [FunctionName("TableOutputFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,[Table("User", "AzureStorageConn")]CloudTable cloudTable)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            User user = JsonConvert.DeserializeObject<User>(requestBody);
            TableOperation operation = TableOperation.Insert(user);
            var result=await cloudTable.ExecuteAsync(operation);
            return new OkObjectResult(result.Result);
        }
    }
}
