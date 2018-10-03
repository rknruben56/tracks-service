namespace SpotPunk.Providers
{
    /// <summary>
    /// Provides the necessary items to perform a search
    /// </summary>
    public interface ISearchTermProvider
    {
        /// <summary>
        /// Returns a random search term
        /// </summary>
        /// <returns></returns>
        string GetRandomSearchTerm();
    }
}
