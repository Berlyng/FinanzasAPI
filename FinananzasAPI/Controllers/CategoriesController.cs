using FinananzasAPI.Application.DTOs.Categories;
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
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //propiedad helper para evitar repetir esta linea
        private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var category = await _categoryService.GetAllAsync(UserId);
            return Ok(ApiResponse<List<CategoryResponse>>.Ok(category));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id, UserId);
            return Ok(ApiResponse<CategoryResponse>.Ok(category));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            var category = await _categoryService.CreateAsync(request, UserId);
            // 201 Created con la URL del recurso creado en el header Location
            return CreatedAtAction(nameof(GetById),
                new { id = category.Id },
                ApiResponse<CategoryResponse>.Ok(category, "Categoría creada"));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, CategoryRequest request)
        {
            var category = await _categoryService.UpdateAsync(id, request, UserId);
            return Ok(ApiResponse<CategoryResponse>.Ok(category, "Categoria Actualizada"));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteAsync(id, UserId);
            return NoContent();
        }

    }
}
