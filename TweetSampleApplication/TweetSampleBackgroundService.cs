using System.Net.Http;
using System.Net.Security;
using MediatR;
using Models;
using System.Threading;
using Microsoft.Extensions.Options;
using TweetSample.Api.Abstraction;
using System.Text;
using System.Text.Json;
using TwitterCommonExtensions;
using Newtonsoft.Json;
using JsonConverter = TwitterCommonExtensions.JsonConverter;
using TweetSample.Api;
using twitter.CommonExtensions;
using TwitterCoreApp;

namespace TweetSampleApplication
{
    public class TweetSampleBackgroundService : BackgroundService
    {
        private readonly ILogger<TweetSampleBackgroundService> _logger;
        private readonly IMediator _mediator;
        private readonly ITwitterAuthClient _twitterAuthClient;

        public TweetSampleBackgroundService(ILogger<TweetSampleBackgroundService> logger, IMediator mediator, ITwitterAuthClient twitterAuthClient)
        {
            _logger = logger;
            _mediator = mediator;
            _twitterAuthClient = twitterAuthClient;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"Begin: ExecuteAsync");
            await foreach (var item in Tweet())
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        TweetSampleModel tweetStream = JsonConverter.ConvertToObject<TweetSampleModel>(item);
                        var command = new TweetStreamSampleCommand
                        {
                            Text = tweetStream.Data?.Text,
                            Id = tweetStream.Data?.Id
                        };
                        await _mediator.Send(command);
                    }
                }
                catch (TweetSampleException ex)
                {
                    this._logger.LogError(ex, $"Error: ExecuteAsync");
                    throw;
                }
            }
        }

        public async IAsyncEnumerable<string> Tweet()
        {
            var httpClient = this._twitterAuthClient.GetTwitterHttpClient();
            using (var stream = await httpClient.GetStreamAsync(httpClient.BaseAddress))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync();
                            yield return line ?? string.Empty;
                        }
                    }
                }
            }
        }
    }
}