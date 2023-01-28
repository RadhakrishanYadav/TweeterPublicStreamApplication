﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using TweetSample.Api.Abstraction;
using TweetSample.Api.Controllers.V2;
using TweetSampleApplication;
using twitter.CommonExtensions;
using TwitterCoreApp.Abstraction;

namespace ApiTest
{
    public class TweetSampleBackgroundServiceTest
    {
        private readonly Mock<IMediator> mockMediatR;
        private readonly Mock<ILogger<TweetSampleBackgroundService>> mockLogger;
        private readonly Mock<ITwitterAuthClient> mockTweeterAutClient;
        private readonly TweetSampleBackgroundService tweetSampleBackgroundService;
        public TweetSampleBackgroundServiceTest()
        {
            this.mockLogger = new Mock<ILogger<TweetSampleBackgroundService>>();
            this.mockMediatR = new Mock<IMediator>();
            this.mockTweeterAutClient = new Mock<ITwitterAuthClient>();
            this.tweetSampleBackgroundService = new TweetSampleBackgroundService(this.mockLogger.Object, this.mockMediatR.Object, this.mockTweeterAutClient.Object);
        }

        [Fact]
        public async Task GetTweeterSampleStream_Success()
        {
            // Arrange
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://twitter/api/stream")
            };
            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"{Constants.TokenBearer} {"token"}");


            this.mockTweeterAutClient
                .Setup(_ => _.GetTwitterHttpClient())
                .Returns(httpClient);

            var stream = new MemoryStream();
            var reader = new StreamReader(stream);
            var actual = reader.ReadToEnd();

            this.mockMediatR.Setup(m => m.Send(It.IsAny<TweetStreamSampleCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            // Act
            await tweetSampleBackgroundService.StartAsync(It.IsAny<CancellationToken>());
        }
    }
}
