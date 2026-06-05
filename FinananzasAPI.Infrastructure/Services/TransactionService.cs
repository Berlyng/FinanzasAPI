using Azure.Core;
using FinananzasAPI.Application.DTOs.Categories;
using FinananzasAPI.Application.DTOs.Transactions;
using FinananzasAPI.Application.Interfaces;
using FinananzasAPI.Domain.Entities;
using FinananzasAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace FinananzasAPI.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _db;

        public TransactionService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<TransactionResponse> CreateAsync(TransactionRequest request, Guid userId)
        {
            var categoryExist = await _db.Categories
                .AnyAsync(c => c.Id == request.CategoryId && c.UserId == userId);

            if (!categoryExist)
            {
                throw new KeyNotFoundException("Categoria no encontrada");
            }

            var transaction = new Transaction
            {
                Description = request.Description,
                Amount = request.Amount,
                Type = request.Type,
                Date = request.Date,
                Notes = request.Notes,
                CategoryId = request.CategoryId,
                UserId = userId
            };

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();

            //Recargarmos la categoria para devolver el response completo
            await _db.Entry(transaction).Reference(t => t.Category).LoadAsync();
            return ToResponse(transaction);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var transaction = await _db.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId)
                ?? throw new KeyNotFoundException("Transacion no encontrada");

             _db.Transactions.Remove(transaction);
            await _db.SaveChangesAsync();
        }

        public async Task<PageResponse<TransactionResponse>> GetAllAsync(Guid userId, TransactionFilters filters)
        {
            // Construimos la query base — EF no ejecuta nada hasta el ToListAsync
            // Esto se llama "deferred execution" y es clave para los filtros dinámicos
            var query =  _db.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId)
                .AsQueryable();

            //Aplicamos filtros solo si vienen en el request
            if (filters.From.HasValue)
            {
                query = query.Where(t => t.Date >= filters.From.Value);
            }

            if (filters.To.HasValue)
            {
                query = query.Where(t => t.Date <= filters.To.Value);
            }

            if (filters.CategoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == filters.CategoryId.Value);
            }

            if (filters.Type.HasValue)
            {
                query = query.Where(t => t.Type == filters.Type.Value);
            }

            if (!string.IsNullOrWhiteSpace(filters.Search))
            {
                query = query.Where(t => t.Description.Contains(filters.Search));
            }

            //Contamos Antes de paginar para saber el total real
            var totalAcount = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.Date)
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .Select(t => ToResponse(t))
                .ToListAsync();

            return new PageResponse<TransactionResponse>
            {
                Items = items,
                TotalAcount = totalAcount,
                Page = filters.Page,
                PageSize = filters.PageSize,
            };
               
        }

        public async Task<TransactionResponse> GetByIdAsync(Guid id, Guid userId)
        {
            var transaction = await _db.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId)
                ?? throw new KeyNotFoundException("Transaccion no encontrada");

            return ToResponse(transaction);
        }

        public async Task<TransactionResponse> UpdateAsync(Guid id, TransactionRequest request, Guid userId)
        {
            var transaction = await _db.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId)
                ?? throw new KeyNotFoundException("Transaccion no encontrada");

            var categoryExist = await _db.Categories
                .AnyAsync(c => c.Id == request.CategoryId && c.UserId == userId);

            if (!categoryExist)
            {
                throw new KeyNotFoundException("Categoria no encontrada");
            }

            transaction.Description = request.Description;
            transaction.Amount = request.Amount;
            transaction.Type = request.Type;
            transaction.Date = request.Date;
            transaction.Notes = request.Notes;
            transaction.CategoryId = request.CategoryId;
            transaction.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            await _db.Entry(transaction).Reference(t => t.Category).LoadAsync();
            return ToResponse(transaction);
        }


        private static TransactionResponse ToResponse(Transaction t) => new()
        {
            Id = t.Id,
            Description = t.Description,
            Amount = t.Amount,
            Type = t.Type,
            Date = t.Date,
            Notes = t.Notes,
            CreatedAt = t.CreatedAt,
            CategoryId = t.CategoryId,
            CategoryName = t.Category.Name,
            CategoryColor = t.Category.Color,
            CategoryIcon = t.Category.Icon
        };
    }
}
