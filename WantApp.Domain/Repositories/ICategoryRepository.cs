using WantApp.Domain.Models.Product;

namespace WantApp.Domain.Repositories;

public interface ICategoryRepository
{
    void AddCategory(Category category);
    Task<IEnumerable<Category>> ListAllCategory();
}
