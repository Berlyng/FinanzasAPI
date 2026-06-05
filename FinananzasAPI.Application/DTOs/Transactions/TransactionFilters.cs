using FinananzasAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Application.DTOs.Transactions
{
    public class TransactionFilters
    {
        public DateTime? From {  get; set; }
        public DateTime? To { get; set; }
        public Guid? CategoryId { get; set; }
        public TransactionType? Type { get; set; }
        public string? Search {  get; set; }


        //Paginacion
        public int Page {  get; set; }
        public int PageSize { get; set; }
    }


    //Respuesta paginada generica para usar en otros endpoints
    public class PageResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalAcount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalAcount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;
    }
}
