using System.Collections.Generic;

namespace SpotPunk.Models
{
    /// <summary>
    /// Search Catalog object
    /// </summary>
    public class SearchCatalog : ISearchCatalog
    {
        #region Properties

        /// <summary>
        /// List of possible search terms
        /// </summary>
        public IList<string> SearchTerms { get; set; }

        #endregion
    }
}
