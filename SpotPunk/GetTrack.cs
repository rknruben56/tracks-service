using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SpotPunk
{
    /// <summary>
    /// Calls the Music API and gets a track to play
    /// </summary>
    public static class GetTrack
    {
        /// <summary>
        /// Azure Function async call
        /// </summary>
        /// <param name="req">request object</param>
        /// <param name="log">logger</param>
        /// <returns></returns>
        [FunctionName("GetTrack")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req, TraceWriter log)
        {
            try
            {
                var client = new HttpClient();
                var trackID = req.Query["track"];
                var userToken = req.Query["token"];

                // Check required parameters (Should they come from the header or queryString??)
                if (string.IsNullOrEmpty(trackID) || string.IsNullOrEmpty(userToken))
                {
                    return GetBadRequest(trackID, userToken);
                }
                else
                {
                    // Set path
                    var baseUri = new Uri(GetEnvironmentVariable("trackUrl"));
                    var fullPath = new Uri(baseUri, trackID);

                    // Set headers
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {req.Query["token"]}");

                    // Get the track object from the Web API and returns it
                    var track = await client.GetStringAsync(fullPath);
                    return new OkObjectResult(track);
                }
            }
            catch(Exception e)
            {
                log.Info($"GetTrack - Error getting track: {e.Message}");
                return new BadRequestObjectResult("Oops! You don goofed");
            }
        }

        /// <summary>
        /// Gets the environment variable value
        /// </summary>
        /// <param name="name">Name of environment variable</param>
        /// <returns></returns>
        public static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
        
        /// <summary>
        /// Returns a bad request based on the null value
        /// </summary>
        /// <param name="trackID"></param>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public static IActionResult GetBadRequest(string trackID, string userToken)
        {
            var errorMessage = string.IsNullOrEmpty(trackID) ? "Please provide a track" : "Please provide a user token";
            return new BadRequestObjectResult(errorMessage);
        }
    }
}
