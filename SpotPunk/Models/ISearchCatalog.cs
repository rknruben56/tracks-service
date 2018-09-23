using System.Collections.Generic;

namespace SpotPunk.Models
{
    /// <summary>
    /// Search catalog object
    /// </summary>
    public interface ISearchCatalog
    {
        #region Properties
        
        /// <summary>
        /// List of possible search terms
        /// </summary>
        IList<string> SearchTerms { get; set; }
        
        #endregion
    }
}
