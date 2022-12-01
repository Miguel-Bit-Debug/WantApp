using Microsoft.EntityFrameworkCore;
using WantApp.Domain.Models.Product;
using WantApp.Domain.Repositories;
using WantApp.InfraData.Data;

namespace WantApp.InfraData.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddCategory(Category category)
        {
            _dbContext.Add(category);
            _dbContext.SaveChanges();
        }

        public async Task<IEnumerable<Category>> ListAllCategory()
        {
            return  _dbContext
                .Categories
                .OrderBy(x => x.CreatedOn)
                .ToList();
        }
    }
}
