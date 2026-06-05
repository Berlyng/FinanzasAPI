using FinananzasAPI.Application.DTOs.Transactions;
using FinananzasAPI.Application.Interfaces;
using FinananzasAPI.Domain.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinananzasAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Los filtros vienen como query params:
        // GET /api/transactions?from=2024-01-01&to=2024-01-31&type=2&page=1&pageSize=20
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TransactionFilters filters)
        {
            var result = await _transactionService.GetAllAsync(UserId, filters);
            return Ok(ApiResponse<PageResponse<TransactionResponse>>.Ok(result));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var transaction = await _transactionService.GetByIdAsync(id, UserId);
            return Ok(ApiResponse<TransactionResponse>.Ok(transaction));
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransactionRequest request)
        {
            var transaction = await _transactionService.CreateAsync(request, UserId);
            return CreatedAtAction(nameof(GetById),
                new { id = transaction.Id },
                ApiResponse<TransactionResponse>.Ok(transaction, "Tranaction creada"));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, TransactionRequest request)
        {
            var transaction = await _transactionService.UpdateAsync(id, request, UserId);
            return Ok(ApiResponse<TransactionResponse>.Ok(transaction, "Transaccion actualizada"));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _transactionService.DeleteAsync(id, UserId);
            return NoContent();
        }
    }
}
