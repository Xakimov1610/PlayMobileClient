using System.Text.Json.Serialization;

namespace PlayMobile.Models.Dtos;

    public class SmsMessagesDto
    {
        [JsonPropertyName("messages")]
        public List<SmsMessageDto>? Messages { get; set; }
    }

    public class SmsMessageDto
    {
        [JsonPropertyName("recipient")]
        public string? Recipient { get; set; }

        [JsonPropertyName("message-id")]
        public string? MessageId { get; set; }

        [JsonPropertyName("sms")]
        public SmsDto? Sms { get; set; }

        [JsonPropertyName("error")]
        public ErrorResponse? Error { get; set; }
    }

    public class SmsDto
    {
        [JsonPropertyName("originator")]
        public string? Originator { get; set; }

        [JsonPropertyName("content")]
        public SmsContent? Content { get; set; }
    }

    public class SmsContent
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }

    public class ErrorResponse
    {
        [JsonPropertyName("error-code")]
        public int Code { get; set; }

        [JsonPropertyName("error-description")]
        public string? Description { get; set; }
    }
