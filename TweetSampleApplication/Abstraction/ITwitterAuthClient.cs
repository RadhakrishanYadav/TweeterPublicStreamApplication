
namespace TweetSample.Api.Abstraction
{
    public interface ITwitterAuthClient
    {
        HttpClient GetTwitterHttpClient();
    }
}
