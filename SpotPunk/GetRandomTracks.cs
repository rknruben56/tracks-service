using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using SpotPunk.Providers;
using SpotPunk.Services;
using System;
using System.Threading.Tasks;

namespace SpotPunk
{
    /// <summary>
    /// Calls the Music API and gets random tracks
    /// </summary>
    public static class GetRandomTracks
    {
        #region Constants, Variables, and Enums

        private static readonly int SearchOffset = 100;
        private static readonly int DefaultNumOfTracks = 5;

        #endregion

        #region Methods

        /// <summary>
        /// Azure Function async call
        /// </summary>
        /// <param name="req">request object</param>
        /// <param name="log">logger</param>
        /// <returns></returns>
        [FunctionName("tracks")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req, TraceWriter log)
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
                    if (!string.IsNullOrEmpty(req.Query["count"]))
                    {
                        int.TryParse(req.Query["count"], out searchCount);
                    }

                    // Get a random searchTerm
                    var searchTerm = new SearchTermProvider().GetRandomSearchTerm();

                    // Call the music service for tracks
                    var tracks = await new SpotifyService().SearchAsync(userToken, searchTerm, searchCount);

                    return new OkObjectResult(tracks);
                }
            }
            catch(Exception e)
            {
                log.Info($"GetTrack - Error getting tracks: {e.Message}");
                return new BadRequestObjectResult("Oops! You don goofed");
            }
        }
        
        #endregion
    }
}
