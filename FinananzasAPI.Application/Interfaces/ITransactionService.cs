using FinananzasAPI.Application.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<PageResponse<TransactionResponse>> GetAllAsync(Guid userId, TransactionFilters filters);
        Task<TransactionResponse> GetByIdAsync(Guid id, Guid userId);
        Task<TransactionResponse> CreateAsync(TransactionRequest request, Guid userId);
        Task<TransactionResponse> UpdateAsync(Guid id, TransactionRequest request, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);

    }
}
