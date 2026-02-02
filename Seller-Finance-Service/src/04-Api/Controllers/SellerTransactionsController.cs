using Microsoft.AspNetCore.Mvc;
using Seller_Finance_Service.src._02_Application.DTOs.Requests;
using Seller_Finance_Service.src._02_Application.DTOs.Responses;
using Seller_Finance_Service.src._02_Application.Services.Interfaces;

namespace Seller_Finance_Service.src._04_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerTransactionsController : ControllerBase
    {
        private readonly ISellerTransactionApplicationService _sellerTransactionApplicationService;

        public SellerTransactionsController(ISellerTransactionApplicationService sellerTransactionApplicationService)
        {
            _sellerTransactionApplicationService = sellerTransactionApplicationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SellerTransactionResponseDto>>> GetTransactions([FromQuery] GetSellerTransactionsRequestDto request)
        {
            var result = await _sellerTransactionApplicationService.GetTransactionsAsync(request);
            return Ok(result);
        }
    }
}
