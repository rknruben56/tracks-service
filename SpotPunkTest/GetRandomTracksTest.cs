using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpotPunk;
using SpotPunk.Providers;
using SpotPunk.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotPunkTest
{
    /// <summary>
    /// Test class for GetRandomTracks Azure Function
    /// </summary>
    [TestClass]
    public class GetRandomTracksTest
    {
        /// <summary>
        /// Verifies GetRandomTracks returns a valid response
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetRandomTracks_SuccessAsync()
        {
            // Arrange
            var request = new Mock<HttpRequest>();
            var logger = new Mock<ILogger>();

            // set request header
            request.Setup(req => req.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>() {
                ["token"] = "UserTokenGoesHere"}
            ));         

            // set music api return string
            var musicService = new Mock<IMusicService>();
            musicService.Setup(service => service.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult("{}")); // TODO: Add Spotify return JSON

            // set search term
            var searchTermProvider = new Mock<ISearchTermProvider>();
            searchTermProvider.Setup(provider => provider.GetRandomSearchTerm()).Returns(It.IsAny<string>());

            // Act
            var message = await GetRandomTracks.RunAsync(request.Object, logger.Object, musicService.Object, searchTermProvider.Object) as OkObjectResult;

            // Assert
            Assert.AreEqual("{}", message.Value); // TODO: Assert Spotify JSON equals function JSON
        }
    }
}
