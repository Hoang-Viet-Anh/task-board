using System.Net.Http.Headers;
using MediatR;
using Microsoft.Extensions.Configuration;
using TaskBoard.Application.Common.Interfaces;

namespace TaskBoard.Infrastructure.Services;

public class SlackService : ISlackService
{
    public readonly IConfiguration _configuration;
    public readonly HttpClient _httpClient;

    public SlackService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task SendMessage(string message)
    {
        var token = _configuration["SlackApp:Token"] ?? throw new Exception("Slack app token is missing");
        var channelId = _configuration["SlackApp:ChannelId"] ?? throw new Exception("Slack app channel id is missing");

        var url = $"https://slack.com/api/chat.postMessage?channel={channelId}&text={message}";

        var request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await _httpClient.SendAsync(request);

    }
}