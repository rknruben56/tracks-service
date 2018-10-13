using Indigo.Functions.Injection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SpotPunk.Providers;
using SpotPunk.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SpotPunk
{
    /// <summary>
    /// Calls a Music API and gets random tracks
    /// </summary>
    public static class GetRandomTracks
    {

        private static readonly int DefaultNumOfTracks = 5;        

        /// <summary>
        /// Azure Function async call
        /// </summary>
        /// <param name="req">request object</param>
        /// <param name="logger">logger</param>
        /// <returns></returns>
        [FunctionName("tracks")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req, 
            ExecutionContext executionContext,
            ILogger logger, 
            [Inject] IMusicService musicService,
            [Inject] ISearchTermProvider searchTermProvider)
        {
            try
            {

                var userToken = req.Headers["token"];

                // Check required parameters
                if (string.IsNullOrEmpty(userToken))
                {
                    return new BadRequestObjectResult("Please provide a user token");
                }
                else
                {
                    // Get user's count request if any
                    int searchCount = DefaultNumOfTracks;
                    if (req.Query != null && !string.IsNullOrEmpty(req.Query["count"]))
                    {
                        if (int.TryParse(req.Query["count"], out searchCount) && searchCount < 0)
                        {
                            return new BadRequestObjectResult("Please enter a valid count.");
                        }
                    }

                    // Get a random searchTerm
                    var searchTerm = searchTermProvider.GetRandomSearchTerm(executionContext);

                    // Call the music service for tracks
                    var musicServiceResponse = await musicService.SearchAsync(userToken, searchTerm, searchCount);

                    if (musicServiceResponse.Item1 == HttpStatusCode.OK)
                    {
                        // Return tracks JSON
                        return new OkObjectResult(musicServiceResponse.Item2);
                    }
                    else
                    {
                        return new BadRequestObjectResult(musicServiceResponse.Item2);
                    }
                }
            }
            catch(Exception e)
            {
                logger.LogInformation($"GetTrack - Error getting tracks: {e.Message}");
                return new BadRequestObjectResult("Oops! You don goofed");
            }
        }
    }
}
