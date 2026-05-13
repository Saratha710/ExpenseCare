using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using ExpenseCareApi.Core.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseCareApi.Controllers;

[Authorize(Roles = "Admin, Trustee, User")]
[ApiController]
[Route("api/[controller]")]
public class RazorpayController : ControllerBase
{
    private readonly IConfiguration _config;

    public RazorpayController(IConfiguration config)
    {
        _config = config;
    }

    // POST /api/razorpay/create-order
    [HttpPost("create-order")]
    public IActionResult CreateOrder([FromBody] CreateOrderDto dto)
    {
        var keyId     = _config["Razorpay:KeyId"];
        var keySecret = _config["Razorpay:KeySecret"];

        var client = new RazorpayClient(keyId, keySecret);

        if(dto.Amount < 50)
         {
            return BadRequest(new { message = "Minimum donation amount is ₹50." });
         }

        var options = new Dictionary<string, object>
        {
            { "amount",   (int)(dto.Amount * 100) }, // Razorpay uses paise
            { "currency", "INR" },
            { "receipt",  $"rcpt_{DateTime.UtcNow.Ticks}" },
            { "notes", new Dictionary<string, string>
                {
                    { "userId",    dto.UserId.ToString() },
                    { "donorName", dto.DonorName }
                }
            }
        };

        var order = client.Order.Create(options);

        return Ok(new
        {
            orderId   = order["id"].ToString(),
            amount    = dto.Amount,
            currency  = "INR",
            keyId     = keyId   // frontend needs this to open checkout
        });
    }
}
