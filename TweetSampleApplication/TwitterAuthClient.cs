using MediatR;
using Microsoft.Extensions.Options;
using Models;
using TweetSample.Api.Abstraction;
using twitter.CommonExtensions;
using Microsoft.Net.Http.Headers;

namespace TweetSample.Api

{
    public class TwitterAuthClient : ITwitterAuthClient
    {
        private readonly ILogger<TwitterAuthClient> _logger;
        private readonly TwitterApiCredential _twitterCredentials;

        public TwitterAuthClient(ILogger<TwitterAuthClient> logger, IMediator mediator, IOptions<TwitterApiCredential> credentialOptions)
        {
            _logger = logger;
            _twitterCredentials = credentialOptions.Value;
        }

        public HttpClient GetTwitterHttpClient()
        {
            _logger.LogDebug($"Begin: GetTwitterHttpClient");
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_twitterCredentials.Url)
            };
            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"{Constants.TokenBearer} {_twitterCredentials.BearerToken}");
            _logger.LogDebug($"End: GetTwitterHttpClient");
            return httpClient;

        }
    }
}
