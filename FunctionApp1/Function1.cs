using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic body = JsonConvert.DeserializeObject(requestBody);
            string csvContent = body?.content;
            if (csvContent == null)
            {
                return new BadRequestObjectResult("Please pass a name on the query string or in the request body");
            }
            byte[] data = Convert.FromBase64String(csvContent);
            string decodedString = System.Text.Encoding.UTF8.GetString(data);
            string[] dataArray = decodedString.Split(new[] { "\n" }, StringSplitOptions.None);
            long result = 0;
            for (int i = 0; i < dataArray.Length - 1; i++)
            {
                result += Int32.Parse(dataArray[i]);
            }
            return (ActionResult)new OkObjectResult($"Result: , {result}");
        }
    }
}
