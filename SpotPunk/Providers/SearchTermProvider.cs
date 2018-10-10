using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using SpotPunk.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpotPunk.Providers
{
    /// <summary>
    /// Provides a random search term
    /// </summary>
    public class SearchTermProvider : ISearchTermProvider
    {
        /// <summary>
        /// Returns a random search term
        /// </summary>
        /// <returns></returns>
        public string GetRandomSearchTerm(ExecutionContext executionContext)
        {
            var searchTerms = new List<string>();
            using (StreamReader r = new StreamReader(Path.Combine(executionContext.FunctionAppDirectory, "searchTerms.json")))
            {
                var json = r.ReadToEnd();
                searchTerms = JsonConvert.DeserializeObject<SearchCatalog>(json).SearchTerms.ToList();
            }
            var randomIndex = new Random().Next(0, searchTerms.Count);
            return searchTerms[randomIndex];
        }
    }
}
