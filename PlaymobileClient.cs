
using Microsoft.Extensions.Options;
using PlayMobile.Models;
using PlayMobile.Models.Dtos;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace PlayMobile;

public class PlaymobileClient : IPlaymobileClient
{
    private readonly ILogger<PlaymobileClient> _logger;
    private readonly HttpClient _client;
    private readonly Options _options;

    private const string SEND_URL = "/broker-api/send";

    public PlaymobileClient(ILogger<PlaymobileClient> logger, HttpClient client, IOptionsMonitor<Options> optionsMonitor)
    {
        _logger = logger;
        _client = client;
        _options = optionsMonitor.CurrentValue;
        _client.BaseAddress = new Uri(_options.BaseUrl);
        var authString = $"{_options.Login}:{_options.Password}";
        var authBase64String = Convert.ToBase64String(
           Encoding.ASCII.GetBytes(authString)
        );
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic",
            authBase64String
        );
    }

    public async Task<Response> SendSmsAsync(string phone, string message)
    {
        try
        {
            var json = JsonSerializer.Serialize(CreateSmsMessageDto(phone, message));

            using var response = await _client.PostAsync(SEND_URL, new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json));

            if (response.StatusCode != HttpStatusCode.OK)
                return new Response
                {
                    IsSuccess = false,
                    ErrorMessage = response.ReasonPhrase
                };

            return new Response
            {
                IsSuccess = true
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(message: $"Playmobile-Error: {exception}");

            return new Response
            {
                IsSuccess = false,
                ErrorMessage = exception.Message
            };
        }
    }

    private SmsMessagesDto CreateSmsMessageDto(string phone, string message)
    {
        return new SmsMessagesDto
        {
            Messages = new List<SmsMessageDto>()
            {
                new()
                {
                    MessageId = Guid.NewGuid().ToString("N"),
                    Recipient = phone,
                    Sms = new SmsDto
                    {
                        Originator = _options.Originator,
                        Content = new SmsContent { Text = message }
                    }
                }
            }
        };
    }
}