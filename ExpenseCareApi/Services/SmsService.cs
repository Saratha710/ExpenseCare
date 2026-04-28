using System.Net.Http;
using Microsoft.Extensions.Configuration;

public class SmsService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public SmsService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task SendOtp(string mobile, string otp)
    {
        var apiKey = _config["Sms:ApiKey"];

        var url = $"https://www.fast2sms.com/dev/bulkV2" +
                  $"?authorization={apiKey}" +
                  $"&route=q" +
                  $"&message=Your OTP is {otp}" +
                  $"&numbers={mobile}";

        var response = await _httpClient.GetAsync(url);

        var result = await response.Content.ReadAsStringAsync();

        Console.WriteLine(result);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"SMS failed: {result}");
        }
    } 
    
}