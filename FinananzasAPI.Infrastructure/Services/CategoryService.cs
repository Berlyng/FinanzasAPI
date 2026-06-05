using FinananzasAPI.Application.DTOs.Categories;
using FinananzasAPI.Application.Interfaces;
using FinananzasAPI.Domain.Entities;
using FinananzasAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinananzasAPI.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _db;

        public CategoryService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CategoryResponse> CreateAsync(CategoryRequest request, Guid userId)
        {
            var exists = await _db.Categories
            .AnyAsync(c => c.Name == request.Name && c.UserId == userId);

            if(exists)
            {
                throw new InvalidOperationException($"Ya existe una categoria con el nombre de: {request.Name}");
            }

            var category = new Category
            {
                Name = request.Name,
                Color = request.Color,
                Icon = request.Icon,
                UserId = userId,
                IsDefault = false
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return ToResponse(category);
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var category = await _db.Categories
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId)
                ?? throw new KeyNotFoundException("Categoria no encontrada");


            if (category.IsDefault)
                throw new InvalidOperationException("Las categorias predeterminaas no se pueden eliminar");

            if (category.Transactions.Any())
                throw new InvalidOperationException($"No puedes eliminar esta categoria por que tiene{category.Transactions.Count} transaciones asociadas. Reasignalas primer.");

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }

        public async Task<List<CategoryResponse>> GetAllAsync(Guid userId)
        {
            return await _db.Categories
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Name)
                .Select(c => ToResponse(c))
                .ToListAsync();

        }

        //Metodo estatico privado - Mapeo del DTO
        private static CategoryResponse ToResponse(Category c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            Color = c.Color,
            Icon = c.Icon,
            isDefault = c.IsDefault,
            CreatedAt = c.CreatedAt,
        };
        

        public async Task<CategoryResponse> GetByIdAsync(Guid id, Guid userId)
        {
            var category = await _db.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId)
                ?? throw new KeyNotFoundException("Categoria no encontrada");
            return ToResponse(category);
        }

        public async Task<CategoryResponse> UpdateAsync(Guid id, CategoryRequest request, Guid userId)
        {
            var category = await _db.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId)
                ?? throw new KeyNotFoundException("Categoría no encontrada");

            if (category.IsDefault)
            {
                throw new InvalidOperationException("Las categorias predeterminadas no se pueden editar");
            }

            var nameExist = await _db.Categories
                .AnyAsync(c => c.Name == request.Name && c.UserId == userId && c.Id != id);

            if (nameExist)
            {

                throw new InvalidOperationException($"Ya existe una categoria con el nombre de: {request.Name}");
            }

            category.Name = request.Name;
            category.Color = request.Color;
            category.Icon = request.Icon;
            category.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return ToResponse(category);
        }
    }
}
