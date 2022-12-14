using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WantApp.Domain.Repositories;

namespace WantApp.API.Controllers.Category;

[Authorize]
[Route("v1/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpPost("add-category")]
    public IActionResult AddCategory([FromBody] Domain.Models.Product.Category category)
    {

        var userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        category.CreatedBy = userId;
        if(!category.IsValid)
        {
            return BadRequest(category.Notifications);
        }

        _categoryRepository.AddCategory(category);

        return Ok(category);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategory()
    {
        var categories = await _categoryRepository.ListAllCategory();

        return Ok(categories);
    }
}
