using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace SpotPunk
{
    /// <summary>
    /// Calls the Music API and gets a track to play
    /// </summary>
    public static class GetRandomTracks
    {
        #region Constants, Variables, and Enums

        private static readonly int SearchOffset = 100;
        private static string[] SearchTerms = { "rock", "blues", "rap", "jazz", "pop", "techno" };

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
                var client = new HttpClient();
                var userToken = req.Headers["token"];

                // Check required parameters
                if (string.IsNullOrEmpty(userToken))
                {
                    return new BadRequestObjectResult("Please provide a user token");
                }
                else
                {
                    // Get url with the proper query string
                    var fullUrl = int.TryParse(req.Query["count"], out int searchLimit) ? GetUrl(searchLimit) : GetUrl();

                    // Set headers
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");

                    // Get the track object from the Web API and return it
                    var track = await client.GetStringAsync(fullUrl);
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
        /// Returns a full url with the query string built
        /// </summary>
        /// <param name="limit">number of tracks to return</param>
        /// <returns></returns>
        private static string GetUrl(int limit = 5)
        {
            // Build search url                   
            var builder = new UriBuilder(GetEnvironmentVariable("searchUrl"));
            builder.Port = -1;

            // Build query string 
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["type"] = "track";

            // Use a random search term, set the random offset, and limit the number of tracks to return
            query["q"] = GetSearchTerm();
            query["offset"] = new Random().Next(SearchOffset).ToString();
            query["limit"] = limit.ToString();
            builder.Query = query.ToString();
            
            return builder.ToString();
        }

        /// <summary>
        /// Gets the environment variable value
        /// </summary>
        /// <param name="name">Name of environment variable</param>
        /// <returns></returns>
        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }

        /// <summary>
        /// Gets the random search term
        /// </summary>
        /// <returns></returns>
        private static string GetSearchTerm()
        {
            var randomIndex = new Random().Next(0, SearchTerms.Length);
            return SearchTerms[randomIndex];
        }

        #endregion
    }
}
