using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpotPunk;
using SpotPunk.Providers;
using SpotPunk.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SpotPunkTest
{
    /// <summary>
    /// Test class for GetRandomTracks Azure Function
    /// </summary>
    [TestClass]
    public class GetRandomTracksTest
    {

        #region Test Data

        private static readonly string ExampleOutput = "{\"tracks\":{\"href\":\"https://api.spotify.com/v1/search?query=cold&type=track&market=US&offset=85&limit=1\",\"items\":[{\"album\":{\"album_type\":\"single\",\"artists\":[{\"external_urls\":{\"spotify\":\"https://open.spotify.com/artist/2RJWS2Lmkw2uExDmFMe1Ry\"},\"href\":\"https://api.spotify.com/v1/artists/2RJWS2Lmkw2uExDmFMe1Ry\",\"id\":\"2RJWS2Lmkw2uExDmFMe1Ry\",\"name\":\"LiL Lotus\",\"type\":\"artist\",\"uri\":\"spotify:artist:2RJWS2Lmkw2uExDmFMe1Ry\"}],\"available_markets\":[\"AD\",\"AR\",\"AT\",\"AU\",\"BE\",\"BG\",\"BO\",\"BR\",\"CA\",\"CH\",\"CL\",\"CO\",\"CR\",\"CY\",\"CZ\",\"DE\",\"DK\",\"DO\",\"EC\",\"EE\",\"ES\",\"FI\",\"FR\",\"GB\",\"GR\",\"GT\",\"HK\",\"HN\",\"HU\",\"ID\",\"IE\",\"IL\",\"IS\",\"IT\",\"JP\",\"LI\",\"LT\",\"LU\",\"LV\",\"MC\",\"MT\",\"MX\",\"MY\",\"NI\",\"NL\",\"NO\",\"NZ\",\"PA\",\"PE\",\"PH\",\"PL\",\"PT\",\"PY\",\"RO\",\"SE\",\"SG\",\"SK\",\"SV\",\"TH\",\"TR\",\"TW\",\"US\",\"UY\",\"VN\",\"ZA\"],\"external_urls\":{\"spotify\":\"https://open.spotify.com/album/105Aso4HZFWGJQUuF4HV9u\"},\"href\":\"https://api.spotify.com/v1/albums/105Aso4HZFWGJQUuF4HV9u\",\"id\":\"105Aso4HZFWGJQUuF4HV9u\",\"images\":[{\"height\":640,\"url\":\"https://i.scdn.co/image/474ff912720d0036fc4b82a9509336a630c2e651\",\"width\":640},{\"height\":300,\"url\":\"https://i.scdn.co/image/afdcfcb491f00f9a0e86f87d1b8b4c88a69c47c0\",\"width\":300},{\"height\":64,\"url\":\"https://i.scdn.co/image/d178d8b77bc5ca364f84a2251c4a695cc3ad85fb\",\"width\":64}],\"name\":\"Bodybag\",\"release_date\":\"2017-07-07\",\"release_date_precision\":\"day\",\"total_tracks\":6,\"type\":\"album\",\"uri\":\"spotify:album:105Aso4HZFWGJQUuF4HV9u\"},\"artists\":[{\"external_urls\":{\"spotify\":\"https://open.spotify.com/artist/2RJWS2Lmkw2uExDmFMe1Ry\"},\"href\":\"https://api.spotify.com/v1/artists/2RJWS2Lmkw2uExDmFMe1Ry\",\"id\":\"2RJWS2Lmkw2uExDmFMe1Ry\",\"name\":\"LiL Lotus\",\"type\":\"artist\",\"uri\":\"spotify:artist:2RJWS2Lmkw2uExDmFMe1Ry\"},{\"external_urls\":{\"spotify\":\"https://open.spotify.com/artist/1fsCfvdiomqjKJFR6xI8e4\"},\"href\":\"https://api.spotify.com/v1/artists/1fsCfvdiomqjKJFR6xI8e4\",\"id\":\"1fsCfvdiomqjKJFR6xI8e4\",\"name\":\"Cold Hart\",\"type\":\"artist\",\"uri\":\"spotify:artist:1fsCfvdiomqjKJFR6xI8e4\"},{\"external_urls\":{\"spotify\":\"https://open.spotify.com/artist/0lGIw6TiVbdnlQFpUGleOY\"},\"href\":\"https://api.spotify.com/v1/artists/0lGIw6TiVbdnlQFpUGleOY\",\"id\":\"0lGIw6TiVbdnlQFpUGleOY\",\"name\":\"Nedarb\",\"type\":\"artist\",\"uri\":\"spotify:artist:0lGIw6TiVbdnlQFpUGleOY\"}],\"available_markets\":[\"AD\",\"AR\",\"AT\",\"AU\",\"BE\",\"BG\",\"BO\",\"BR\",\"CA\",\"CH\",\"CL\",\"CO\",\"CR\",\"CY\",\"CZ\",\"DE\",\"DK\",\"DO\",\"EC\",\"EE\",\"ES\",\"FI\",\"FR\",\"GB\",\"GR\",\"GT\",\"HK\",\"HN\",\"HU\",\"ID\",\"IE\",\"IL\",\"IS\",\"IT\",\"JP\",\"LI\",\"LT\",\"LU\",\"LV\",\"MC\",\"MT\",\"MX\",\"MY\",\"NI\",\"NL\",\"NO\",\"NZ\",\"PA\",\"PE\",\"PH\",\"PL\",\"PT\",\"PY\",\"RO\",\"SE\",\"SG\",\"SK\",\"SV\",\"TH\",\"TR\",\"TW\",\"US\",\"UY\",\"VN\",\"ZA\"],\"disc_number\":1,\"duration_ms\":283404,\"explicit\":false,\"external_ids\":{\"isrc\":\"QM42K1783407\"},\"external_urls\":{\"spotify\":\"https://open.spotify.com/track/48AnfHcY01HOX0lHngb8ir\"},\"href\":\"https://api.spotify.com/v1/tracks/48AnfHcY01HOX0lHngb8ir\",\"id\":\"48AnfHcY01HOX0lHngb8ir\",\"is_local\":false,\"name\":\"Bodybag (Feat. cold hart)\",\"popularity\":55,\"preview_url\":\"https://p.scdn.co/mp3-preview/2f899c5f07d23ee14edbae90c4cfdf53b7f0cc3a?cid=774b29d4f13844c495f206cafdad9c86\",\"track_number\":1,\"type\":\"track\",\"uri\":\"spotify:track:48AnfHcY01HOX0lHngb8ir\"}],\"limit\":1,\"next\":\"https://api.spotify.com/v1/search?query=cold&type=track&market=US&offset=86&limit=1\",\"offset\":85,\"previous\":\"https://api.spotify.com/v1/search?query=cold&type=track&market=US&offset=84&limit=1\",\"total\":95548}}";
        private static readonly string UnauthorizedOutput = "Unauthorized fool.";

        #endregion

        /// <summary>
        /// Verifies that a bad request is returned when no user token is received
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetRandomTracks_NoTokenReturnsBadRequest_Async()
        {
            // Arrange
            var request = new Mock<HttpRequest>();
            var logger = new Mock<ILogger>();

            // set music api return value
            var musicService = new Mock<IMusicService>();
            musicService.Setup(service => service.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(Tuple.Create(HttpStatusCode.OK, "test")));

            // set search term
            var searchTermProvider = new Mock<ISearchTermProvider>();
            searchTermProvider.Setup(provider => provider.GetRandomSearchTerm()).Returns(It.IsAny<string>());

            // Act
            var result = await GetRandomTracks.RunAsync(request.Object, logger.Object, musicService.Object, searchTermProvider.Object);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

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
            request.Setup(req => req.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>()
            {
                ["token"] = "UserTokenGoesHere"
            }
            ));

            // set music api return value
            var musicService = new Mock<IMusicService>();
            musicService.Setup(service => service.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(Tuple.Create(HttpStatusCode.OK, ExampleOutput)));

            // set search term
            var searchTermProvider = new Mock<ISearchTermProvider>();
            searchTermProvider.Setup(provider => provider.GetRandomSearchTerm()).Returns(It.IsAny<string>());

            // Act
            var result = await GetRandomTracks.RunAsync(request.Object, logger.Object, musicService.Object, searchTermProvider.Object) as OkObjectResult;

            // Assert
            Assert.AreEqual(ExampleOutput, result.Value);
        }

        /// <summary>
        /// Verifies a bad request is returned when there was an error with the Music API
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetRandomTracks_MusicAPIErrorBadRequestAsync()
        {
            // Arrange
            var request = new Mock<HttpRequest>();
            var logger = new Mock<ILogger>();

            // set request header
            request.Setup(req => req.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>()
            {
                ["token"] = "UserTokenGoesHere"
            }
            ));

            // set music api return value
            var musicService = new Mock<IMusicService>();
            musicService.Setup(service => service.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(Tuple.Create(HttpStatusCode.Unauthorized, UnauthorizedOutput)));

            // set search term
            var searchTermProvider = new Mock<ISearchTermProvider>();
            searchTermProvider.Setup(provider => provider.GetRandomSearchTerm()).Returns(It.IsAny<string>());

            // Act
            var result = await GetRandomTracks.RunAsync(request.Object, logger.Object, musicService.Object, searchTermProvider.Object) as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(UnauthorizedOutput, result.Value);
        }
    }
}
