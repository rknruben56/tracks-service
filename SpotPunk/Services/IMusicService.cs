namespace SpotPunk.Services
{
    /// <summary>
    /// Music Service Interface
    /// </summary>
    public interface IMusicService
    {
        #region Methods

        /// <summary>
        /// Searches the Music API for random tracks
        /// </summary>
        /// <returns></returns>
        System.Threading.Tasks.Task<string> SearchAsync(string userToken, string searchTerm, int limit);

        #endregion
    }
}
