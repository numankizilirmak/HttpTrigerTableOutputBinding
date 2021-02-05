using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Storage.Queue;

namespace HttpTrigerTableOutputBinding
{
    public static class TableOutputFunction
    {
        [FunctionName("TableOutputFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,[Table("User", "AzureStorageConn")]CloudTable cloudTable, [Queue("user",Connection = "AzureStorageConn")]CloudQueue cloudQueue)
        {            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            User user = JsonConvert.DeserializeObject<User>(requestBody);
            TableOperation operation = TableOperation.Insert(user);
            var result=await cloudTable.ExecuteAsync(operation);

            var userJson = JsonConvert.SerializeObject(user);
            CloudQueueMessage cloudQueueMessage = new CloudQueueMessage(userJson);
            await cloudQueue.AddMessageAsync(cloudQueueMessage);

            return new OkObjectResult(result.Result);
        }
    }
}
