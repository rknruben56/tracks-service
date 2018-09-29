using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace SpotPunk.Services
{
    public class SpotifyService : IMusicService
    {
        #region Constants, Enums, and Variables

        private static readonly int SearchOffset = 100;

        #endregion

        #region Constructors
        
        /// <summary>
        /// TODO: Inject HttpClient
        /// </summary>
        public SpotifyService() { }
        #endregion

        #region Methods

        /// <summary>
        /// Searches the Spotify API for random tracks
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<string> SearchAsync(string userToken, string searchTerm, int limit)
        {
            // Get url with the proper query string
            var url = GetUrl(searchTerm, limit);

            // Set headers
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");

            // Call the API and return tracks
            var tracks = await client.GetStringAsync(url);
            return tracks;
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

        #endregion

    }
}
