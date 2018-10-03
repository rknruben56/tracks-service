using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace SpotPunk.Services
{
    public class SpotifyService : IMusicService
    {

        private static readonly int SearchOffset = 100;

        /// <summary>
        /// Searches the Spotify API for random tracks
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns>Tuple where Item 1 is the Http Status code and Item 2 is the result JSON</returns>
        public async System.Threading.Tasks.Task<Tuple<HttpStatusCode, string>> SearchAsync(string userToken, string searchTerm, int limit)
        {
            // Get url with the proper query string
            var url = GetUrl(searchTerm, limit);

            // Set headers
            var client = new HttpClient(); // TODO: Inject?
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");

            // Call the API and return tracks
            var result = await client.GetAsync(url);
            var response = await result.Content.ReadAsStringAsync();
            return Tuple.Create(result.StatusCode, response);
        }

        /// <summary>
        /// Returns a full url with the query string built
        /// </summary>
        /// <param name="limit">number of tracks to return</param>
        /// <returns></returns>
        private static string GetUrl(string searchTerm, int limit = 5)
        {
            // Build search url                   
            var builder = new UriBuilder(GetEnvironmentVariable("searchUrl"));
            builder.Port = -1;

            // Build query string 
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["type"] = "track";

            // Use a random search term, set the random offset, and limit the number of tracks to return
            query["q"] = searchTerm;
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
    }
}
