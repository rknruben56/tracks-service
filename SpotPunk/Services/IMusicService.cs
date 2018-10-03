using System;
using System.Net;

namespace SpotPunk.Services
{
    /// <summary>
    /// Music Service Interface
    /// </summary>
    public interface IMusicService
    {
        /// <summary>
        /// Searches the Music API for random tracks
        /// </summary>
        /// <returns>Tuple where Item 1 is the Http Status code and Item 2 is the result JSON</returns>
        System.Threading.Tasks.Task<Tuple<HttpStatusCode, string>> SearchAsync(string userToken, string searchTerm, int limit);
    }
}
